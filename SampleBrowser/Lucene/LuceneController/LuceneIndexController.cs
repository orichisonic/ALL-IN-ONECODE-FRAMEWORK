using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using SampleBrowser.Common;
using SamplesController.Services;
using SamplesModel.Entities;
using SampleBrowser.Lucene.LuceneIndexs;


namespace SampleBrowser.Lucene.LuceneController
{
    public class LuceneIndexController
    {
        /// <summary>
        /// Create all samples index
        /// </summary>
        public static void CreateLuceneIndex()
        {
            string indexFilePath = Constants.INDEX_FILE_PATH;
            string xmlPath = Constants.XML_SOURCE_FILE_PATH;
            

            string sourceDirectory = string.Empty;
            string slnPath = string.Empty;

            string referSampleDirectoryName = string.Empty;
            string newSourceDirectory = string.Empty;
            List<string> newSourceDirectoryList = new List<string>();

            List<Sample> lstSamples = SamplesService.GetAllSamples(xmlPath); //Get all samples from xml
            
            List<string> lstAllReferSamples = new List<string>();   //temp sample list to store reference samples in order to judge whether the sample is repeat.

            LuceneIndex luceneIndex = new LuceneIndex();
            foreach (Sample sam in lstSamples)
            {
                sourceDirectory = Path.Combine(indexFilePath, sam.VsVersion == 2008 ? "Visual Studio 2008" : "Visual Studio 2010", sam.Name);
                //WriteToIndexForSample(sourceDirectory, sam, luceneIndex);   //TODO: (filter repeated samples, may need be removed)create sample to document and add it to index

                if (sam.ReferSamples != null && sam.ReferSamples.Count > 0)
                {
                    foreach (string rSam in sam.ReferSamples)
                    {
                        if (rSam.Contains("\\"))
                        {
                            referSampleDirectoryName = rSam.Substring(0, rSam.IndexOf("\\"));
                            if (!newSourceDirectoryList.Contains(referSampleDirectoryName))
                            {
                                newSourceDirectoryList.Add(referSampleDirectoryName);
                            }

                            //if (!lstAllReferSamples.Contains(referSampleDirectoryName)) //TODO:(filter repeated samples, may need be removed) create sample to document and add it to index
                            //{
                                //lstAllReferSamples.Add(referSampleDirectoryName);   //TODO: (filter repeated samples, may need be removed)create sample to document and add it to index
                                if (newSourceDirectoryList != null && newSourceDirectoryList.Count > 0)
                                {
                                    foreach (string referSamDic in newSourceDirectoryList)
                                    {
                                        newSourceDirectory = sourceDirectory.Replace(sam.Name, referSamDic);
                                        WriteToIndexForSample(newSourceDirectory, sam, luceneIndex, false);   //create referenceSample's each file to document and add it to index
                                        WriteToIndexForSample(newSourceDirectory, sam, luceneIndex, true);  //create referenceSamples to document and add it to index
                                    }
                                }
                            //}
                        }
                    }
                    referSampleDirectoryName = string.Empty;
                    newSourceDirectory = string.Empty;
                    newSourceDirectoryList.Clear();
                }
            }
            luceneIndex.Optimize();
            luceneIndex.Close();
        }

        /// <summary>
        /// Create document and write it to index for sample 
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="sam"></param>
        /// <param name="luceneIndex"></param>
        /// <param name="onlySampleInfo">true: search txtCondtion is Null Or Empty</param>
        private static void WriteToIndexForSample(string sourceDirectory, Sample sam, LuceneIndex luceneIndex, bool onlySampleInfo)
        {
            string slnFullPath = string.Concat(sourceDirectory, ".sln");
            string slnPath = slnFullPath.Replace(Constants.INDEX_FILE_PATH, "");

            if (!onlySampleInfo)
            {
                string[] fileExtends = { ".cs", ".cpp", ".vb", ".aspx", ".txt", ".h", ".js", ".xaml" };

                DirectoryInfo di = new DirectoryInfo(sourceDirectory);
                FileInfo[] fis = di.GetFiles("*", SearchOption.AllDirectories);
                foreach (FileInfo fi in fis)
                {
                    if (fileExtends.Contains(fi.Extension))
                    {
                        luceneIndex.WriteToIndex(fi, sam, slnPath, onlySampleInfo);  //add document for each file to index
                    }
                }
            }
            else
            {
                luceneIndex.WriteToIndex(null, sam, slnPath, onlySampleInfo);  //add document for each sample to index
            }
        }
    }
}

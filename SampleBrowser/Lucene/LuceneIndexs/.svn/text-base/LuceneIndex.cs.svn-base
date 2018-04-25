using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Lucene.Net.Index;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;

using SampleBrowser.Common;
using SamplesModel.Entities;
using SamplesModel.Enum;
using SampleBrowser.Lucene;


namespace SampleBrowser.Lucene.LuceneIndexs
{
    public class LuceneIndex
    {
        IndexWriter writer = null;
        string INDEX_STORE_PATH = Constants.INDEX_STORE_PATH;
        StandardAnalyzer standarAnalyzer = LuceneAnalyzer.GetStandardAnalyzer();

        public LuceneIndex()
        {
            if (!Directory.Exists(INDEX_STORE_PATH))
            {
                Directory.CreateDirectory(INDEX_STORE_PATH);
            }
            writer = new IndexWriter(INDEX_STORE_PATH, standarAnalyzer, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isCreate">Is create or update index</param>
        public LuceneIndex(bool isCreate)
        {
            if (!Directory.Exists(INDEX_STORE_PATH))
            {
                Directory.CreateDirectory(INDEX_STORE_PATH);
            }
            writer = new IndexWriter(INDEX_STORE_PATH, standarAnalyzer, isCreate);
        }

        /// <summary>
        /// Create document for each sample
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="sample"></param>
        /// <param name="slnPath"></param>
        /// <param name="onlySampleInfo">true:search txtCondtion is Null Or Empty</param>
        /// <returns></returns>
        private Document CreateDocument(FileInfo fi, Sample sample, string slnPath, bool onlySampleInfo)
        {
            Document doc = new Document();
            string filePath = string.Empty;

            if (fi != null)
            {
                doc.Add(new Field("contents", new StreamReader(fi.FullName, Encoding.Default)));

                filePath = fi.FullName.Replace(Constants.INDEX_FILE_PATH, "");
            }
            else
            {
                doc.Add(new Field("contents", "", Field.Store.YES, Field.Index.UN_TOKENIZED));
            }

            doc.Add(new Field("filePath", filePath, Field.Store.YES, Field.Index.UN_TOKENIZED));

            doc.Add(new Field("sampleName", sample.Name, Field.Store.YES, Field.Index.UN_TOKENIZED));
            doc.Add(new Field("description", sample.Description, Field.Store.YES, Field.Index.TOKENIZED));

            foreach (Technology tech in sample.Technologies)
            {
                doc.Add(new Field("technologies", (EnumOperator.GetTechnologyStringValue(tech)).ToLower(), Field.Store.YES, Field.Index.UN_TOKENIZED));
            }

            if (!string.IsNullOrEmpty(sample.CreateDate.ToString()))
            {
                string createDate = DateTools.DateToString(DateTime.Parse(sample.CreateDate.ToString()), DateTools.Resolution.MILLISECOND);
                doc.Add(new Field("createDate", createDate, Field.Store.YES, Field.Index.UN_TOKENIZED));
            }
            else
            {
                doc.Add(new Field("createDate", "", Field.Store.YES, Field.Index.UN_TOKENIZED));
            }

            if (!string.IsNullOrEmpty(sample.UpdateDate.ToString()))
            {
                string updateDate = DateTools.DateToString(DateTime.Parse(sample.UpdateDate.ToString()), DateTools.Resolution.MILLISECOND);
                doc.Add(new Field("updateDate", sample.UpdateDate.ToString(), Field.Store.YES, Field.Index.UN_TOKENIZED));
            }
            else
            {
                doc.Add(new Field("updateDate", "", Field.Store.YES, Field.Index.UN_TOKENIZED));
            }

            string language = EnumOperator.GetLanguageStringValue((Language)sample.Language);
            doc.Add(new Field("language", (EnumOperator.GetLanguageStringValue((Language)sample.Language)).ToLower(), Field.Store.YES, Field.Index.UN_TOKENIZED));
            doc.Add(new Field("vs", sample.VsVersion.ToString(), Field.Store.YES, Field.Index.UN_TOKENIZED));

            if (sample.Tags != null && sample.Tags.Count > 0)
            {
                foreach (string tag in sample.Tags)
                {
                    doc.Add(new Field("tags", tag, Field.Store.YES, Field.Index.TOKENIZED));
                }
            }

            doc.Add(new Field("difficulty", sample.Difficulty, Field.Store.YES, Field.Index.UN_TOKENIZED));

            doc.Add(new Field("slnPath", slnPath, Field.Store.YES, Field.Index.NO));
            doc.Add(new Field("readMePath", sample.ReadMePath, Field.Store.YES, Field.Index.NO));

            doc.Add(new Field("onlySampleInfo", onlySampleInfo ? "true" : "false",
                Field.Store.YES, Field.Index.UN_TOKENIZED));

            return doc;
        }

        /// <summary>
        /// Write document to new index 
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="sample"></param>
        /// <param name="slnPath"></param>
        /// <param name="onlySampleInfo">true: search txtCondtion is Null Or Empty</param>
        public void WriteToIndex(FileInfo fi, Sample sample, string slnPath, bool onlySampleInfo)
        {
            Document doc = CreateDocument(fi, sample, slnPath, onlySampleInfo);
            writer.SetMergeFactor(1000);
            writer.SetMaxMergeDocs(Int32.MaxValue);
            writer.AddDocument(doc);
        }

        /// <summary>
        /// Optimize index
        /// </summary>
        public void Optimize()
        {
            writer.Optimize();
        }

        /// <summary>
        /// Close IndexWriter
        /// </summary>
        public void Close()
        {
            writer.Close();
        }
    }
}

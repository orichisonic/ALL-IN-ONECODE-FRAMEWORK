using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using SamplesModel.Entities;
using SamplesModel.Enum;
using SamplesController.Interfaces;
using SampleBrowser.SamplesModel.Entities;
using System.IO;
using SampleBrowser.Common;

namespace SamplesController.Controller
{
    public class SampleController : ISamplesControl
    {
        /// <summary>
        /// Get all samples as search result
        /// </summary>
        /// <param name="samples"></param>
        /// <returns></returns>
        public List<SearchResult> GetAllSearchResults(List<Sample> samples)
        {
            List<SearchResult> lstSearchResults = new List<SearchResult>();
            SearchResult sr = null;

            foreach (Sample san in samples)
            {
                sr = new SearchResult();
                sr.SlnPath = Path.Combine(san.VsVersion == 2008 ? "Visual Studio 2008" : "Visual Studio 2010", san.Name + ".sln");
                sr.FilePath = Path.Combine(san.VsVersion == 2008 ? "Visual Studio 2008" : "Visual Studio 2010", san.Name);
                sr.Snippet = san.Description;
                sr.Samples = san;
                lstSearchResults.Add(sr);
            }

            return lstSearchResults;
        }


        /// <summary>
        /// Get all samples from XML
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public List<Sample> GetAllSamples(string xmlPath)
        {
            List<Sample> listSamples = new List<Sample>();

            string tempDateTime;
            string language;
            string vsVersion;
            string tags;
            SearchResult sr = null;

            XElement xelement = XElement.Load(xmlPath);

            var query = xelement.Descendants("CodeSample").AsParallel();
            if (query != null && query.Count() > 0)
            {
                foreach (var item in query)
                {
                    sr = new SearchResult();

                    Sample sample = new Sample();

                    sample.Name = item.Attribute("Name").Value;

                    tempDateTime = item.Attribute("CreateDate").Value;
                    if (!string.IsNullOrEmpty(tempDateTime))
                    {
                        sample.CreateDate = DateTime.Parse(tempDateTime);
                    }

                    tempDateTime = item.Attribute("UpdateDate").Value;
                    if (!string.IsNullOrEmpty(tempDateTime))
                    {
                        sample.UpdateDate = DateTime.Parse(tempDateTime);
                    }

                    language = item.Attribute("Language").Value;
                    if (!string.IsNullOrEmpty(language))
                    {
                        sample.Language = EnumOperator.GetLanguageEnumValue(language);
                    }
                    else
                    {
                        sample.Language = Language.Others;
                    }


                    vsVersion = item.Attribute("VS").Value;
                    if (!string.IsNullOrEmpty(vsVersion))
                    {
                        sample.VsVersion = int.Parse(vsVersion);
                    }

                    tags = item.Attribute("Tags").Value;
                    if (!string.IsNullOrEmpty(tags))
                    {
                        sample.Tags = new List<string>();
                        string[] str = tags.Split(',');
                        foreach (string s in str)
                        {
                            sample.Tags.Add(s);
                        }
                    }

                    sample.Difficulty = item.Attribute("Difficulty").Value;
                    sample.DownloadLink = item.Attribute("DownloadLink").Value;
                    sample.ReadMeLink = item.Attribute("ReadMeLink").Value;
                    sample.ReadMePath = item.Attribute("ReadMePath").Value;
                    sample.Description = item.Element("Description").Value;

                    var teches = item.Descendants("Technology").AsParallel().Select(t => t.Value);
                    if (teches != null && teches.Count() > 0)
                    {
                        sample.Technologies = new List<Technology>();
                        foreach (var tc in teches)
                        {
                            sample.Technologies.Add(EnumOperator.GetTechnologyEnumValue(tc));
                        }
                    }

                    var referSamples = item.Descendants("Sample").AsParallel().Select(t => t.Attribute("PhysicalPath").Value);
                    if (referSamples != null && referSamples.Count() > 0)
                    {
                        sample.ReferSamples = new List<string>();
                        foreach (string rSam in referSamples)
                        {
                            sample.ReferSamples.Add(rSam);
                        }
                    }

                    listSamples.Add(sample);
                }
            }
            else
            {
                listSamples = null;
            }

            return listSamples;
        }
    }
}

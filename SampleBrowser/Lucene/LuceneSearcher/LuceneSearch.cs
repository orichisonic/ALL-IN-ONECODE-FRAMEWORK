using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using SampleBrowser.Common;
using SamplesModel.Entities;
using SampleBrowser.SamplesModel.Entities;
using SamplesModel.Enum;
using SampleBrowser.Lucene;

using Lucene.Net.Search;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Highlight;
using Lucene.Net.Analysis;
using SampleBrowser.Common.MultiThread;
using System.Collections.ObjectModel;


namespace SampleBrowser.Lucene.LuceneSearcher
{
    public class LuceneSearch
    {
        private IndexSearcher searcher = null;
        StandardAnalyzer standardAnalyzer = LuceneAnalyzer.GetStandardAnalyzer();
        Highlighter highlighter = null;

        public LuceneSearch()
        {            
            searcher = new IndexSearcher(IndexReader.Open(Constants.INDEX_STORE_PATH));
        }

        /// <summary>
        /// Search results
        /// </summary>
        /// <param name="txt">Search condition TextBox's value</param>
        /// <param name="languages">Search codintion about language</param>
        /// <param name="technologies">Search codintion about technology</param>
        /// <returns></returns>
        public ObservableList<SearchResult> SearchResults(string txt, string languages, string technologies)
        {
            BooleanQuery queryBoolMain = new BooleanQuery();
            BooleanQuery queryBoolTxt = null;
            BooleanQuery queryBoolLanguages = null;
            BooleanQuery queryBoolTechnologies = null;

            Query queryLanguage = null;
            Query queryTechnologies = null;
            Query queryMulty = null;

            if (!string.IsNullOrEmpty(txt))
            {
                queryBoolTxt = new BooleanQuery();

                MultiFieldQueryParser multiParser = new MultiFieldQueryParser(new String[] { "contents" }, standardAnalyzer);
                //MultiFieldQueryParser multiParser = new MultiFieldQueryParser(new String[] { "contents", "description" }, standardAnalyzer);
                queryMulty = multiParser.Parse(txt);

                Query queryPrefix = new PrefixQuery(new Term("sampleName", txt));

                Query querySpecialFiledTxt = new TermQuery(new Term("onlySampleInfo", "false"));

                queryBoolTxt.Add(queryMulty, BooleanClause.Occur.SHOULD);
                queryBoolTxt.Add(queryPrefix, BooleanClause.Occur.SHOULD);
                queryBoolTxt.Add(querySpecialFiledTxt, BooleanClause.Occur.MUST);

                queryBoolMain.Add(queryBoolTxt, BooleanClause.Occur.MUST);
            }

            if (string.IsNullOrEmpty(languages) && languages != "All Languages")
            {
                queryBoolLanguages = new BooleanQuery();

                string[] LanArr = languages.Split(',');
                foreach (string lan in LanArr)
                {
                    queryLanguage = new TermQuery(new Term("language", lan.ToLower()));
                    queryBoolLanguages.Add(queryLanguage, BooleanClause.Occur.SHOULD);
                }

                Query querySpecialFiledLan = new TermQuery
                            (new Term("onlySampleInfo", string.IsNullOrEmpty(txt) ? "true" : "false"));
                queryBoolLanguages.Add(querySpecialFiledLan, BooleanClause.Occur.MUST);

                queryBoolMain.Add(queryBoolLanguages, BooleanClause.Occur.MUST);
            }

            if (!string.IsNullOrEmpty(technologies) && technologies != "All Technologies")
            {
                queryBoolTechnologies = new BooleanQuery();

                string[] TechArr = technologies.Split(',');
                foreach (string tech in TechArr)
                {
                    queryTechnologies = new TermQuery(new Term("technologies", tech.ToLower()));
                    queryBoolTechnologies.Add(queryTechnologies, BooleanClause.Occur.SHOULD);
                }

                Query querySpecialFiledTech = new TermQuery
                            (new Term("onlySampleInfo", string.IsNullOrEmpty(txt) ? "true" : "false"));
                queryBoolTechnologies.Add(querySpecialFiledTech, BooleanClause.Occur.MUST);

                queryBoolMain.Add(queryBoolTechnologies, BooleanClause.Occur.MUST);
            }

            if (string.IsNullOrEmpty(txt) && (string.IsNullOrEmpty(languages) || languages == "All Languages")
                && (string.IsNullOrEmpty(technologies) || technologies == "All Technologies"))
            {
                Query queryAll = new TermQuery(new Term("onlySampleInfo", "true"));
                queryBoolMain.Add(queryAll, BooleanClause.Occur.SHOULD);
            }

            Hits h = searcher.Search(queryBoolMain);
            ObservableList<SearchResult> lst = new ObservableList<SearchResult>(System.Windows.Threading.Dispatcher.CurrentDispatcher);

            if (h.Length() == 0)
            {
                return null;
            }
            else
            {
                for (int i = 0; i < h.Length(); i++)
                {
                    try
                    {
                        Document doc = h.Doc(i);

                        SearchResult sr = new SearchResult();
                        sr.FilePath = string.IsNullOrEmpty(txt) ? "" : doc.Get("filePath");                      

                        string resultText = string.Empty;
                        if (!string.IsNullOrEmpty(txt))
                        {
                            if (queryBoolTxt != null)
                            {
                                StreamReader streamReader = new StreamReader(Path.Combine(Constants.INDEX_FILE_PATH, sr.FilePath.Trim('\\')), Encoding.Default);
                                string contentStr = streamReader.ReadToEnd();

                                //SimpleHTMLFormatter simpleHTMLFormatter = new SimpleHTMLFormatter("<font color=\"red\">", "</font>");
                                SimpleHTMLFormatter simpleHTMLFormatter = new SimpleHTMLFormatter("", "");
                                highlighter = new Highlighter(simpleHTMLFormatter, new QueryScorer(queryMulty));
                               
                                highlighter.SetTextFragmenter(new SimpleFragmenter(200));
                                TokenStream tokenStream = standardAnalyzer.TokenStream("contents", new StringReader(contentStr));
                                resultText = highlighter.GetBestFragment(tokenStream, contentStr);
                            }
                        }
                        sr.Snippet = string.IsNullOrEmpty(resultText) ? "" : resultText;

                        Sample sample = new Sample();

                        sample.Name = doc.Get("sampleName");
                        sample.Description = doc.Get("description");
                        sample.VsVersion = int.Parse(doc.Get("vs"));
                        sample.Difficulty = doc.Get("difficulty");
                        sample.Language = EnumOperator.GetLanguageEnumValue(doc.Get("language"));
                        string[] technolos = doc.GetValues("technologies");
                        sample.Technologies = new List<Technology>();
                        foreach (string tec in technolos)
                        {
                            sample.Technologies.Add(EnumOperator.GetTechnologyEnumValue(tec));
                        }

                        sample.ReadMePath = doc.Get("readMePath");
                        sr.SlnPath = doc.Get("slnPath");
                        sr.Score = h.Score(i);

                        sr.Samples = sample;
                        lst.Add(sr);
                    }
                    catch (FileNotFoundException)
                    {
                        // ...
                    }
                }
               return lst;
            }
        }

        /// <summary>
        /// Search results modify by Jason Wang
        /// </summary>
        /// <param name="txt">Search condition TextBox's value</param>
        /// <param name="languages">Search codintion about language</param>
        /// <param name="technologies">Search codintion about technology</param>
        /// <returns></returns>
        public IEnumerable<SearchResult> SearchResultsIterList(string txt, string languages, string technologies)
        {
            BooleanQuery queryBoolMain = new BooleanQuery();
            BooleanQuery queryBoolTxt = null;
            BooleanQuery queryBoolLanguages = null;
            BooleanQuery queryBoolTechnologies = null;

            Query queryLanguage = null;
            Query queryTechnologies = null;
            Query queryMulty = null;

            SearchResult sr = new SearchResult();
            if (!string.IsNullOrEmpty(txt))
            {
                queryBoolTxt = new BooleanQuery();

                //MultiFieldQueryParser multiParser = new MultiFieldQueryParser(new String[] { "contents", "description" }, standardAnalyzer);
                MultiFieldQueryParser multiParser = new MultiFieldQueryParser(new String[] { "contents" }, standardAnalyzer);
                queryMulty = multiParser.Parse(txt);

                Query queryPrefix = new PrefixQuery(new Term("sampleName", txt));

                Query querySpecialFiledTxt = new TermQuery(new Term("onlySampleInfo", "false"));

                queryBoolTxt.Add(queryMulty, BooleanClause.Occur.SHOULD);
                queryBoolTxt.Add(queryPrefix, BooleanClause.Occur.SHOULD);
                //queryBoolTxt.Add(querySpecialFiledTxt, BooleanClause.Occur.MUST);

                queryBoolMain.Add(querySpecialFiledTxt, BooleanClause.Occur.MUST);
                queryBoolMain.Add(queryBoolTxt, BooleanClause.Occur.MUST);
            }
            else
            {
                Query queryAll = new TermQuery(new Term("onlySampleInfo", "true"));
                queryBoolMain.Add(queryAll, BooleanClause.Occur.MUST);
            }

            if (!string.IsNullOrEmpty(languages) && languages != "All Languages")
            {
                queryBoolLanguages = new BooleanQuery();

                string[] LanArr = languages.Split(',');
                foreach (string lan in LanArr)
                {
                    queryLanguage = new TermQuery(new Term("language", lan.ToLower()));
                    queryBoolLanguages.Add(queryLanguage, BooleanClause.Occur.SHOULD);
                }

                queryBoolMain.Add(queryBoolLanguages, BooleanClause.Occur.MUST);
            }

            if (!string.IsNullOrEmpty(technologies) && technologies != "All Technologies")
            {
                queryBoolTechnologies = new BooleanQuery();

                string[] TechArr = technologies.Split(',');
                foreach (string tech in TechArr)
                {
                    queryTechnologies = new TermQuery(new Term("technologies", tech.ToLower()));
                    queryBoolTechnologies.Add(queryTechnologies, BooleanClause.Occur.SHOULD);
                }

                queryBoolMain.Add(queryBoolTechnologies, BooleanClause.Occur.MUST);
            }

            Hits h = searcher.Search(queryBoolMain);
            
            ObservableCollection<SearchResult> lst = new ObservableCollection<SearchResult>();
            if (h.Length() == 0)
            {
                yield return null;
            }
            else
            {
                for (int i = 0; i < h.Length(); i++)
                {
                    Document doc = h.Doc(i);

                    sr.FilePath = string.IsNullOrEmpty(txt) ? null : doc.Get("filePath");

                    string resultText = string.Empty;
                    if (!string.IsNullOrEmpty(txt))
                    {
                        if (queryBoolTxt != null)
                        {
                            StreamReader streamReader = new StreamReader(Path.Combine(Constants.INDEX_FILE_PATH, sr.FilePath.Trim('\\')), Encoding.Default);
                            string contentStr = streamReader.ReadToEnd();

                            //SimpleHTMLFormatter simpleHTMLFormatter = new SimpleHTMLFormatter("<font color=\"red\">", "</font>");
                            SimpleHTMLFormatter simpleHTMLFormatter = new SimpleHTMLFormatter("","");
                           highlighter = new Highlighter(simpleHTMLFormatter, new QueryScorer(queryMulty));
                           
                            highlighter.SetTextFragmenter(new SimpleFragmenter(200));
                            TokenStream tokenStream = standardAnalyzer.TokenStream("contents", new StringReader(contentStr));
                            resultText = highlighter.GetBestFragment(tokenStream, contentStr);
                        }
                    }
                    sr.Snippet = string.IsNullOrEmpty(resultText) ? null : resultText;

                    Sample sample = new Sample();

                    sample.Name = doc.Get("sampleName");
                    sample.Description = doc.Get("description");
                    sample.VsVersion = int.Parse(doc.Get("vs"));
                    sample.Language = EnumOperator.GetLanguageEnumValue(doc.Get("language"));
                    sample.Difficulty = doc.Get("difficulty");
                    string[] technolos = doc.GetValues("technologies");
                    sample.Technologies = new List<Technology>();
                    foreach (string tec in technolos)
                    {
                        sample.Technologies.Add(EnumOperator.GetTechnologyEnumValue(tec));
                    }

                    sample.ReadMePath = doc.Get("readMePath");
                    sr.SlnPath = doc.Get("slnPath");
                    sr.Score = h.Score(i);

                    sr.Samples = sample;
                    lst.Add(sr);

                    yield return sr;
                }
            }
        }

        public void Close()
        {
            searcher.Close();
        }
    }
}

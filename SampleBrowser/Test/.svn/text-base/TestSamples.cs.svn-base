using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SampleBrowser.SamplesModel.Entities;
using SamplesModel.Entities;
using SamplesModel.Enum;

namespace SampleBrowser.Test
{
    public class TestSamples
    {
        public static List<SearchResult> SearchResults
        {
            get
            {
                if (_searchResults == null)
                {
                    _searchResults = GetSearchResults();
                }

                return _searchResults;
            }
        }

        private static List<SearchResult> _searchResults;
        private static List<SearchResult> GetSearchResults()
        {
            List<Technology> lstTech = null;
            _searchResults = new List<SearchResult>();

            Sample sam = new Sample();
            sam.Name = "ATLCOMClient";
            sam.VsVersion = 2008;
            sam.Language = Language.CSharp;
            lstTech = new List<Technology>();
            lstTech.Add(Technology.COM);
            sam.Technologies = lstTech;
            sam.Description = "ATLCOMClient description ATLCOMClient description ATLCOMClient description";

            SearchResult sr = new SearchResult();
            sr.Samples = sam;
            sr.Score = 1.0F;
            sr.Snippet = "ATLCOMClient 2008 2008 2008";
            sr.FilePath = "\\2008\\ATLCOMClient\\acb.aspx";

            _searchResults.Add(sr);

            sam = new Sample();
            sam.Name = "ATLCOMClient";
            sam.VsVersion = 2010;
            sam.Language = Language.CSharp;
            lstTech = new List<Technology>();
            lstTech.Add(Technology.COM);
            sam.Technologies = lstTech;
            sam.Description = "ATLCOMClient description ATLCOMClient description ATLCOMClient description";

            sr = new SearchResult();
            sr.Samples = sam;
            sr.Score = 1.2F;
            sr.Snippet = "ATLCOMClient 2010 2010 2010";
            sr.FilePath = "\\2010\\ATLCOMClient\\ewqewqeweqweqw.txt";

            _searchResults.Add(sr);

            sam = new Sample();
            sam.Name = "ATLShellExtIconOverlayHandler";
            sam.VsVersion = 2008;
            sam.Language = Language.Cpp;
            lstTech = new List<Technology>();
            lstTech.Add(Technology.Library);
            sam.Technologies = lstTech;
            sam.Description = "ATLShellExtIconOverlayHandler description ATLShellExtIconOverlayHandler description ";

            sr = new SearchResult();
            sr.Samples = sam;
            sr.Score = 0.7F;
            sr.Snippet = "ATLShellExtIconOverlayHandler 2008 2008 2008 2008 2008 2008 2008 2008";
            sr.FilePath = "\\2008\\ATLShellExtIconOverlayHandler\\extconOvelay.cpp";

            _searchResults.Add(sr);

            sam = new Sample();
            sam.Name = "ATLShellExtIconOverlayHandler";
            sam.VsVersion = 2008;
            sam.Language = Language.Cpp;
            lstTech = new List<Technology>();
            lstTech.Add(Technology.Library);
            sam.Technologies = lstTech;
            sam.Description = "ATLShellExtIconOverlayHandler description ATLShellExtIconOverlayHandler description ";

            sr = new SearchResult();
            sr.Samples = sam;
            sr.Score = 1.9F;
            sr.Snippet = "ATLShellExtIconOverlayHandler Test Test Test Test Test Test Test Test";
            sr.FilePath = "\\2008\\ATLShellExtIconOverlayHandler\\ovelayHandelrATLShell.cs";

            _searchResults.Add(sr);


            sam = new Sample();
            sam.Name = "ATLShellExtIconOverlayHandler";
            sam.VsVersion = 2010;
            sam.Language = Language.Cpp;
            lstTech = new List<Technology>();
            lstTech.Add(Technology.Library);
            sam.Technologies = lstTech;
            sam.Description = "ATLShellExtIconOverlayHandler description ATLShellExtIconOverlayHandler description ";

            sr = new SearchResult();
            sr.Samples = sam;
            sr.Score = 0.9F;
            sr.Snippet = "ATLShellExtIconOverlayHandler 2010 2010 2010 2010 2010 2010 2010 2010 2010 2010 2010 2010";
            sr.FilePath = "\\2010\\ATLShellExtIconOverlayHandler\\KhAutomatinshggjl.vb";

            _searchResults.Add(sr);

            sam = new Sample();
            sam.Name = "CppAutomateWord";
            sam.VsVersion = 2010;
            sam.Language = Language.VisualBasic;
            lstTech = new List<Technology>();
            lstTech.Add(Technology.Office);
            sam.Technologies = lstTech;
            sam.Description = "CppAutomateWord description CppAutomateWord description CppAutomateWord description ";

            sr = new SearchResult();
            sr.Samples = sam;
            sr.Score = 2.36F;
            sr.Snippet = "CppAutomateWord 2010 2010 CppAutomateWord CppAutomateWord CppAutomateWord CppAutomateWord 2010 2010 2010 2010";
            sr.FilePath = "\\2010\\CppAutomateWord\\TestFilePath\\CppAutomateWord\\CppAutomateWord.aspx";

            _searchResults.Add(sr);

            return _searchResults;
        }
    }
}

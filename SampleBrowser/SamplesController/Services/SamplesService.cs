using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SamplesModel.Entities;
using SamplesController.Controller;
using SampleBrowser.Lucene.LuceneController;
using SampleBrowser.SamplesModel.Entities;

namespace SamplesController.Services
{
    /// <summary>
    /// Services provide methods for out of Controller layer
    /// </summary>
    public class SamplesService
    {
        static SampleController op;

        static SamplesService()
        {
            if (op == null)
            {
                op = new SampleController();
            }
        }

        /// <summary>
        /// Get all samples as search result
        /// </summary>
        /// <param name="samples"></param>
        /// <returns></returns>
        public static List<SearchResult> GetAllSearchResults(List<Sample> samples)
        {
            return op.GetAllSearchResults(samples);
        }

        /// <summary>
        /// Get all samples from XML
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public static List<Sample> GetAllSamples(string xmlPath)
        {
            return op.GetAllSamples(xmlPath);
        }

        /// <summary>
        /// Create all samples index
        /// </summary>
        public static void CreateLuceneIndex()
        {
            LuceneIndexController.CreateLuceneIndex();
        }
    }
}

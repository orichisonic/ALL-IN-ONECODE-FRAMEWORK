using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Analysis.Standard;

namespace SampleBrowser.Lucene
{
    public class LuceneAnalyzer
    {
        static StandardAnalyzer standardAnalyzer;

        public static StandardAnalyzer GetStandardAnalyzer()
        {
            if (standardAnalyzer == null)
                standardAnalyzer = new StandardAnalyzer();
            return standardAnalyzer;
        }
    }
}

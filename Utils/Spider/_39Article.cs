using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Utils.Spider
{
    public class _39Article : IArticle
    {
        public override string xGetContent()
        {
            return @"//div[contains(@id, 'contentText')]/p[position() < last()]";
        }

        public override string xGetLinks()
        {
            return @"//ul[@class='newslist']/li/*/a/@href"; 
        }

        public override string xGetPages()
        {
            return @"//p[contains(@class, 'pageno')]/span[(position() = 2 or position()  = last() - 1)]"; 
        }
    }
}

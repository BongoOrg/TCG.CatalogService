using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCG.CatalogService.Domain.ElasticSearchDto
{

    public class ElasticsearchDocument
    {
        public ElasticsearchSubSource hits { get; set; }
    }

    public class ElasticsearchSubSource
    {
        public ElasticsearchSource hits { get; set; }
    }

    public class ElasticsearchSource
    {
        public ElasticItem _source { get; set; }
    }

    public class ElasticItem
    {
        [PropertyName("mongo_id")]
        public string _id { get; set; }
        [PropertyName("IdExtension")]
        public string IdExtension { get; set; }
        [PropertyName("LibelleExtension")]
        public string LibelleExtension { get; set; }
        [PropertyName("Name")]
        public string Name { get; set; }
        [PropertyName("Image")]
        public string Image { get; set; }
        [PropertyName("Language")]
        public string Language { get; set; } = "fr";
        [PropertyName("IdCard")]
        public string IdCard { get; set; }
    }
}

namespace ExampleMvcTemplate.Infrastructure.ModelMetadata
{
    using System;
    using System.Web.Mvc;

    public class WatermarkAttribute : Attribute, IMetadataAware
    {
        public string PlaceHolder { get; set; }

        public void OnMetadataCreated(System.Web.Mvc.ModelMetadata metadata)
        {
            metadata.Watermark = PlaceHolder;
        }
    }
}
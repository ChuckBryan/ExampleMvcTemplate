namespace ExampleMvcTemplate.Infrastructure.ModelMetadata
{
    using System;
    using System.Collections.Generic;

    public interface IModelMetadataFilter
    {
        void TransformMetadata(System.Web.Mvc.ModelMetadata metadata, IEnumerable<Attribute> attributes);
    }
}
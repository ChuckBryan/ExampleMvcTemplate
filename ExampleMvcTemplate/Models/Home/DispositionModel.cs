namespace ExampleMvcTemplate.Models.Home
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Infrastructure.ModelMetadata;

    public class DispositionModel
    {
        [HiddenInput]
        public int Id { get; set; }
       
        [Required]
        [DataType("DispositionDropDown")]
        public int DispositionId { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Help("Example of a Help Message")]
        [Watermark(PlaceHolder = "Enter your Disposition Note")]
        public string Note { get; set; }

        [Display(Name="Is This Private?")]
        public bool IsThisPrivate { get; set; }
    }
}
namespace Ordering.Core.Dtos.Sample
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public  class SampleDto 
    {
        public Guid SampleId { get; set; }
        public string ExternalId { get; set; }
        public string InternalId { get; set; }
        public string SampleType { get; set; }
        public string ContainerType { get; set; }
        public DateTimeOffset? CollectionDate { get; set; }
        public DateTimeOffset? ArrivalDate { get; set; }
        public int? Amount { get; set; }
        public string AmountUnits { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}
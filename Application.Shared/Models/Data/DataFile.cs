using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Shared.Models.Data;


public enum UploadStatus
{
    Pending,
    Processing,
    Completed,
    Failed
}

public class DataFile
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; } = default!;
    public string? FileName { get; set; } = default!;

    public string? Directory { get; set; } = default!;

    public string? Description { get; set; } = default!;

    public string? Tags { get; set; } = default!; // ex: "tagKey1:tagValue1,tagKey2:tagValue2"

    //[NotMapped]
    //[JsonIgnore]
    //public IDictionary<string, string> Metadata
    //{
    //    get
    //    {
    //        if(!String.IsNullOrEmpty(Tags))
    //        {
    //            return Tags.Split(',').Select(tag => tag.Split(':')).ToDictionary(tag => tag[0], tag => tag[1]);
    //        }
    //        else
    //        {
    //            return new Dictionary<string, string>();
    //        }
                
    //    }
    //}

    [NotMapped]
    public string? Content { get; set; } = default!;


    [NotMapped]
    public UploadStatus? AzureUploadStatus { get; set; } = default!;

    public void SetTags(IDictionary<string, string> tags)
    {
        Tags = string.Join(",", tags.Select(tag => $"{tag.Key}:{tag.Value}"));
    }
}

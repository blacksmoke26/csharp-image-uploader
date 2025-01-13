using System.IO;
using Newtonsoft.Json;
using PixPost.Helpers;

namespace PixPost.Objects.Service;

public static class ServiceFactory {
  public static ImageService CreateFromFile(string jsonFile) {
    return CreateFromText(File.ReadAllText(jsonFile));
  }

  public static string GetServiceSchema() {
    return ResourceHelper.GetTextResource("Resources/Schemas/ImageService.schema.json");
  }

  public static bool ValidateSchemaFile(string schemaJson, out IList<string> errors) {
    return SchemaHelper.ValidateFile(GetServiceSchema(), schemaJson, out errors);
  }

  public static bool ValidateSchemaText(string schemaJson, out IList<string> errors) {
    return SchemaHelper.ValidateText(GetServiceSchema(), schemaJson, out errors);
  }

  public static ImageService CreateFromSchema(ImageSchema schema) => new(schema);

  public static ImageSchema ParseSchema(string schemaJson) {
    var parsed = JsonConvert.DeserializeObject<ImageSchema>(schemaJson)!;
    if (parsed == null) throw new InvalidDataException();
    return parsed;
  }

  public static ImageService CreateFromText(string json) {
    return CreateFromSchema(ParseSchema(json));
  }

  public static string GetServiceText() {
    return """
           {
            "ServiceName": "ImgBb",
            "Title": "ImgBB",
            "Menu": {
              "Text": "ImgBB",
              "Icon": "\uE5D4"
            },
            "Logo": "data:image/jpeg;base64,iVBORw0KGgoAAAANSUhEUgAAAJsAAAAgCAMAAAAysvoIAAAAkFBMVEVHcEwiotkiqOAdk8Qip+Aip94ipt8iqOAAbZMjqN8hpdsiqN8iotgip98ip+AiqOAip+Aio9wip+Aip98ip98hpt4jp90iqOAipt0hpd4ioNohpt4eoNAip+AiqN8ip+Ahpd4dntMiqOAiqN8ip98hpt4ipdwip+Aip98ipuAYisUiqOAiqN8ip98ip98jqOAM5QOUAAAAL3RSTlMAHOMH3E9w+wH+Jc0VXvXwlimEvJJJQKo5NSFZCuvVyEUQ0aNUOy+1aHwDw4yAnJjPk9MAAAKaSURBVHhe3dZbb+IwEAXg4yQEO4SwQCEUEij3e+f//7utx7a8WT8hWVq038v4VEayxjAu/gNn+jHFaz5IGwY5rpKYwEsGpI2DHJcgVuAVaZ9+jGSQIwhasJZ4xYK0aZBjK9ZE6zFekpM2CXJki97hej1seaUVqzxflYA4XHoCDk7DfCV4wxzAnrTHZbUE6+R4EtJKt1IV/ah6ey4TCTbfkUM1gMwF9SER5Lg/08ysvL6tObSLIi8Hyk4Mciw92wtehXYA8MUnVQmxBTAjL0GQY5mQlvOKZWSYk7QARMv3e5QVdzQFnqR9L+66FEGOZUra0a3UWbak/ZIjd9nfpF1QcB0AONsPrXT5CnIsLfeiAcyZ1LLhC6xK8NnugOA/ZBJDc2i4D6VmrB2CHEnhvlWuKzhyPUNw/QAe9tbdm+m3mr2zIEfieuFXOdcnDu4kGWmFfzOxslvNnkWQo/C98Kua68y2a2n71/o302/lPaoJciQb0k64NWvblYTr1nQpcfPh7t/MtNm5Tmbm+7Dp5khu5hWQeBIbSTuLbZdqpMRq/2YOiLUSc12vkFU3R1LYpmwTYlM7gfc4cb3i4s52M2fqnchtvel+9QVENyOSI2mD2Z2MiZvF5irVfjjirNpZTaw8k5F97e0v4NTNsZyoq2dn8QJfXAPJlaj+M28B0c2xpBUZbZ+LkCM7RktFxm7z1/8A6+2anNEnAHRyPBNiyVxx8WMU38SS4rOiP2WC336WnaB1cjzyWRG1j625wtrPYqSTlqhfC2DhO0fVowEwHxCR2uQpDJ/jasZCQgvI5XgLtszIUCWMcrxsYPj8L/gv2Brv5kG0m9jLfjcZ0XxjZ8y7SYha0pIb3s3Zj+a3YyecyvGGxvus39afiOY3shDVlDIzOR8AAAAASUVORK5CYII=",
            "DocsUrl": "https://api.imgbb.com/",
            "Description": "Free image hosting and sharing service, upload pictures, photo host.",
            "ExpirationRange": [
              {
                "Label": "None",
                "Value": "Don't auto delete"
              },
              {
                "Label": "TM5",
                "Value": "After 5 minutes"
              },
              {
                "Label": "TM15",
                "Value": "After 15 minutes"
              },
              {
                "Label": "TM30",
                "Value": "After 30 minutes"
              },
              {
                "Label": "TH1",
                "Value": "After 1 hour"
              },
              {
                "Label": "TH3",
                "Value": "After 3 hours"
              },
              {
                "Label": "TH6",
                "Value": "After 6 hours"
              },
              {
                "Label": "TH12",
                "Value": "After 12 hours"
              },
              {
                "Label": "ED1",
                "Value": "After 1 day"
              },
              {
                "Label": "ED2",
                "Value": "After 2 days"
              },
              {
                "Label": "ED3",
                "Value": "After 3 days"
              },
              {
                "Label": "ED4",
                "Value": "After 4 days"
              },
              {
                "Label": "ED5",
                "Value": "After 5 days"
              },
              {
                "Label": "ED6",
                "Value": "After 6 days"
              },
              {
                "Label": "EW1",
                "Value": "After 1 week"
              },
              {
                "Label": "EW2",
                "Value": "After 2 weeks"
              },
              {
                "Label": "EW3",
                "Value": "After 3 weeks"
              },
              {
                "Label": "EM1",
                "Value": "After 1 month"
              },
              {
                "Label": "EM2",
                "Value": "After 2 months"
              },
              {
                "Label": "EM3",
                "Value": "After 3 months"
              },
              {
                "Label": "EM4",
                "Value": "After 4 months"
              },
              {
                "Label": "EM5",
                "Value": "After 5 months"
              },
              {
                "Label": "EM6",
                "Value": "After 6 months"
              }
            ],
            "VariablePrefix": "IMGBB",
            "Variables": [
              {
                "Name": "APIKEY",
                "Type": "string",
                "IsRequired": true
              }
            ],
            "RequestProps": {
              "Endpoint": "https://api.imgbb.com/1/upload",
              "MimeTypes": ["jpg", "jpeg", "jfif", "png", "gif", "bmp", "webp"],
              "FileSize": {
           	    "Minimum": 1,
           	    "Maximum": 33554432,
              },
              "KeyConfig": {
                "Image": {
                 	"Key": "image",
                 	"IsRequired": true,
                 	"UseInRequest": true
                },
                "Api": {
                 	"Key": "key",
                 	"IsRequired": true,
                 	"UseInRequest": true
                },
                "Caption": {
                 	"Key": "name",
                 	"IsRequired": false,
                 	"UseInRequest": true
                },
                "Expiration": {
                 	"Key": "expiration",
                 	"IsRequired": false,
                 	"UseInRequest": true
                }
              }
            }
           }
           """;
  }
}
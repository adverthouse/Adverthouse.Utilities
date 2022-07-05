using System.Collections.Generic;

namespace Test.WebUI.Models.Services
{
    
    public class SmallJpg
    {
        public int height { get; set; }
        public int width { get; set; }
        public int file_size { get; set; }
        public string display_name { get; set; }
        public int dpi { get; set; }
        public string format { get; set; }
        public bool is_licensable { get; set; }
    }

    public class MediumJpg
    {
        public int height { get; set; }
        public int width { get; set; }
        public int file_size { get; set; }
        public string display_name { get; set; }
        public int dpi { get; set; }
        public string format { get; set; }
        public bool is_licensable { get; set; }
    }

    public class HugeJpg
    {
        public int height { get; set; }
        public int width { get; set; }
        public int file_size { get; set; }
        public string display_name { get; set; }
        public int dpi { get; set; }
        public string format { get; set; }
        public bool is_licensable { get; set; }
    }

    public class SupersizeJpg
    {
        public int height { get; set; }
        public int width { get; set; }
        public int file_size { get; set; }
        public string display_name { get; set; }
        public int dpi { get; set; }
        public string format { get; set; }
        public bool is_licensable { get; set; }
    }

    public class HugeTiff
    {
        public int height { get; set; }
        public int width { get; set; }
        public int file_size { get; set; }
        public string display_name { get; set; }
        public int dpi { get; set; }
        public string format { get; set; }
        public bool is_licensable { get; set; }
    }

    public class SupersizeTiff
    {
        public int height { get; set; }
        public int width { get; set; }
        public int file_size { get; set; }
        public string display_name { get; set; }
        public int dpi { get; set; }
        public string format { get; set; }
        public bool is_licensable { get; set; }
    }

    public class VectorEps
    {
        public int height { get; set; }
        public int width { get; set; }
        public int file_size { get; set; }
        public string display_name { get; set; }
        public int dpi { get; set; }
        public string format { get; set; }
        public bool is_licensable { get; set; }
    }

    public class SmallThumb
    {
        public string url { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }

    public class LargeThumb
    {
        public string url { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }

    public class Preview
    {
        public string url { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }

    public class Preview1000
    {
        public string url { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }

    public class Assets
    {
        public SmallJpg small_jpg { get; set; }
        public MediumJpg medium_jpg { get; set; }
        public HugeJpg huge_jpg { get; set; }
        public SupersizeJpg supersize_jpg { get; set; }
        public HugeTiff huge_tiff { get; set; }
        public SupersizeTiff supersize_tiff { get; set; }
        public VectorEps vector_eps { get; set; }
        public SmallThumb small_thumb { get; set; }
        public LargeThumb large_thumb { get; set; }
        public Preview preview { get; set; }
        public Preview1000 preview_1000 { get; set; }
    }

    public class Model
    {
        public string id { get; set; }
    }

    public class Image
    {
        public string id { get; set; }
        public string description { get; set; }
        public string added_date { get; set; }
        public string media_type { get; set; }
        //  public Contributor contributor { get; set; }
        public double aspect { get; set; }
        public string image_type { get; set; }
        public bool is_editorial { get; set; }
        public bool is_adult { get; set; }
        public bool is_illustration { get; set; }
        public bool has_model_release { get; set; }
        public bool has_property_release { get; set; }
        public List<string> releases { get; set; }
        //     public List<Category> categories { get; set; }
        public List<string> keywords { get; set; }
        public Assets assets { get; set; }
        public List<Model> models { get; set; }
    }

    public class Image2
    {
        public string id { get; set; }
        public string description { get; set; }
        public string added_date { get; set; }
        public string media_type { get; set; }
        //    public Contributor contributor { get; set; }
        public double aspect { get; set; }
        public string image_type { get; set; }
        public bool is_editorial { get; set; }
        public bool is_adult { get; set; }
        public bool is_illustration { get; set; }
        public bool has_model_release { get; set; }
        public bool has_property_release { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public List<string> releases { get; set; }
        //  public List<Category> categories { get; set; }
        public List<string> keywords { get; set; }
        public ImageAssets assets { get; set; }
        public List<Model> models { get; set; }
    }

    public class ImageAssets
    {
        public ImageSizeDetails small_jpg { get; set; }
        public ImageSizeDetails medium_jpg { get; set; }
        public ImageSizeDetails huge_jpg { get; set; }
        public ImageSizeDetails supersize_jpg { get; set; }
        public ImageSizeDetails huge_tiff { get; set; }
        public ImageSizeDetails supersize_tiff { get; set; }
        public ImageSizeDetails vector_eps { get; set; }
        public Thumbnail small_thumb { get; set; }
        public Thumbnail large_thumb { get; set; }
        public Thumbnail preview { get; set; }
        public Thumbnail preview_1000 { get; set; }
    }

    public class Thumbnail
    {
        public string url { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }

    public class ImageSizeDetails
    {
        public int height { get; set; }
        public int width { get; set; }
        public int file_size { get; set; }
        public string display_name { get; set; }
        public int dpi { get; set; }
        public string format { get; set; }
        public bool is_licensable { get; set; }
    }

    public class Suggest
    {
        public List<string> keywords { get; set; }
    }
    public class ImageLst
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total_count { get; set; }
        public string search_id { get; set; }
        public string message { get; set; }
        public List<Image2> data { get; set; }
    }

    public class CategoryList
    {
        //  public List<Category> data { get; set; }
        public int page { get; set; }
        public int per_page { get; set; }
        public int total_count { get; set; }
        public string message { get; set; }
    }
}

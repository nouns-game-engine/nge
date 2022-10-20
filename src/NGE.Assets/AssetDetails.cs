namespace NGE.Assets
{
	public class AssetDetails
	{
		public string? FriendlyName { get; set; }
		public AssetClassification Classification { get; set; }
		public object? Asset { get; set; }
        public string? Path { get; set; }
        public string? InformationalPath { get; set; }
    }
}
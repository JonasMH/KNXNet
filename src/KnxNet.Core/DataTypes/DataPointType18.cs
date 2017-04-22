namespace KnxNet.Core.DataTypes
{
	public class DataPointType18
	{
		/// <summary>
		/// True  - activate the scene corresponding to the field Scene Number
		/// False - Learn the scene corresponding to the field Scene Number
		/// </summary>
		public bool C { get; set; }

		/// <summary>
		/// 0..63
		/// </summary>
		public byte SceneNumber { get; set; }
	}
}
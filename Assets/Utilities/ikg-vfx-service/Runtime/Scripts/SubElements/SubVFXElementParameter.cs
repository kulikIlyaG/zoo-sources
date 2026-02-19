namespace IKGTools.VFXs.SubElements
{
    public abstract class SubVFXElementParameter
    {
        public string Id { get; }

        protected SubVFXElementParameter(string id)
        {
            Id = id;
        }
    }
}
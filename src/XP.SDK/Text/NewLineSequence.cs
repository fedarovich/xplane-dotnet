namespace XP.SDK.Text
{
    /// <summary>
    /// The new-line character sequence to use.
    /// </summary>
    public enum NewLineSequence
    {
        /// <summary>
        /// <c>\r\n</c> on Windows; <c>\n</c> on other platforms.
        /// </summary>
        Auto,
        
        /// <summary>
        /// <c>\r\n</c>
        /// </summary>
        Windows,
        
        /// <summary>
        /// <c>\n</c>
        /// </summary>
        Unix
    }
}

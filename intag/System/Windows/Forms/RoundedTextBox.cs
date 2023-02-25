namespace System.Windows.Forms
{
    internal class RoundedTextBox : TextBox
    {

        [System.Runtime.InteropServices.DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,  
            int nTopRect,   
            int nRightRect, 
            int nBottomRect,
                            
            int nHeightRect,
            int nWeightRect 
        );

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Region = Drawing.Region.FromHrgn(CreateRoundRectRgn(1, 1, Width, Height, 1, 1));
        }
    }
}
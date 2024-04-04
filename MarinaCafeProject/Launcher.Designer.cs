namespace MarinaCafeProject
{
    partial class Launcher
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Launcher));
            this.bunifuFormDock1 = new Bunifu.UI.WinForms.BunifuFormDock();
            this.panel_top = new System.Windows.Forms.Panel();
            this.bunifuButton1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.bunifuLabel1 = new Bunifu.UI.WinForms.BunifuLabel();
            this.tb_null = new System.Windows.Forms.TextBox();
            this.bunifuElipse1 = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.bunifuElipse2 = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.bunifuElipse3 = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.btn_session = new Bunifu.UI.WinForms.BunifuImageButton();
            this.bunifuImageButton1 = new Bunifu.UI.WinForms.BunifuImageButton();
            this.btn_table_management = new Bunifu.UI.WinForms.BunifuImageButton();
            this.btn_sale_history = new Bunifu.UI.WinForms.BunifuImageButton();
            this.btn_product_settings = new Bunifu.UI.WinForms.BunifuImageButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel_top.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // bunifuFormDock1
            // 
            this.bunifuFormDock1.AllowFormDragging = true;
            this.bunifuFormDock1.AllowFormDropShadow = true;
            this.bunifuFormDock1.AllowFormResizing = true;
            this.bunifuFormDock1.AllowHidingBottomRegion = true;
            this.bunifuFormDock1.AllowOpacityChangesWhileDragging = false;
            this.bunifuFormDock1.BorderOptions.BottomBorder.BorderColor = System.Drawing.Color.Silver;
            this.bunifuFormDock1.BorderOptions.BottomBorder.BorderThickness = 1;
            this.bunifuFormDock1.BorderOptions.BottomBorder.ShowBorder = true;
            this.bunifuFormDock1.BorderOptions.LeftBorder.BorderColor = System.Drawing.Color.Silver;
            this.bunifuFormDock1.BorderOptions.LeftBorder.BorderThickness = 1;
            this.bunifuFormDock1.BorderOptions.LeftBorder.ShowBorder = true;
            this.bunifuFormDock1.BorderOptions.RightBorder.BorderColor = System.Drawing.Color.Silver;
            this.bunifuFormDock1.BorderOptions.RightBorder.BorderThickness = 1;
            this.bunifuFormDock1.BorderOptions.RightBorder.ShowBorder = true;
            this.bunifuFormDock1.BorderOptions.TopBorder.BorderColor = System.Drawing.Color.Silver;
            this.bunifuFormDock1.BorderOptions.TopBorder.BorderThickness = 1;
            this.bunifuFormDock1.BorderOptions.TopBorder.ShowBorder = true;
            this.bunifuFormDock1.ContainerControl = this;
            this.bunifuFormDock1.DockingIndicatorsColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(215)))), ((int)(((byte)(233)))));
            this.bunifuFormDock1.DockingIndicatorsOpacity = 0.5D;
            this.bunifuFormDock1.DockingOptions.DockAll = true;
            this.bunifuFormDock1.DockingOptions.DockBottomLeft = true;
            this.bunifuFormDock1.DockingOptions.DockBottomRight = true;
            this.bunifuFormDock1.DockingOptions.DockFullScreen = true;
            this.bunifuFormDock1.DockingOptions.DockLeft = true;
            this.bunifuFormDock1.DockingOptions.DockRight = true;
            this.bunifuFormDock1.DockingOptions.DockTopLeft = true;
            this.bunifuFormDock1.DockingOptions.DockTopRight = true;
            this.bunifuFormDock1.FormDraggingOpacity = 0.9D;
            this.bunifuFormDock1.ParentForm = this;
            this.bunifuFormDock1.ShowCursorChanges = true;
            this.bunifuFormDock1.ShowDockingIndicators = true;
            this.bunifuFormDock1.TitleBarOptions.AllowFormDragging = true;
            this.bunifuFormDock1.TitleBarOptions.BunifuFormDock = this.bunifuFormDock1;
            this.bunifuFormDock1.TitleBarOptions.DoubleClickToExpandWindow = true;
            this.bunifuFormDock1.TitleBarOptions.TitleBarControl = this.panel_top;
            this.bunifuFormDock1.TitleBarOptions.UseBackColorOnDockingIndicators = false;
            // 
            // panel_top
            // 
            this.panel_top.AllowDrop = true;
            this.panel_top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(106)))), ((int)(((byte)(105)))));
            this.panel_top.Controls.Add(this.bunifuButton1);
            this.panel_top.Controls.Add(this.bunifuLabel1);
            this.panel_top.Controls.Add(this.tb_null);
            this.panel_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_top.Location = new System.Drawing.Point(0, 0);
            this.panel_top.Name = "panel_top";
            this.panel_top.Size = new System.Drawing.Size(616, 35);
            this.panel_top.TabIndex = 17;
            // 
            // bunifuButton1
            // 
            this.bunifuButton1.AllowAnimations = true;
            this.bunifuButton1.AllowMouseEffects = true;
            this.bunifuButton1.AllowToggling = false;
            this.bunifuButton1.AnimationSpeed = 200;
            this.bunifuButton1.AutoGenerateColors = false;
            this.bunifuButton1.AutoRoundBorders = false;
            this.bunifuButton1.AutoSizeLeftIcon = true;
            this.bunifuButton1.AutoSizeRightIcon = true;
            this.bunifuButton1.BackColor = System.Drawing.Color.Transparent;
            this.bunifuButton1.BackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(106)))), ((int)(((byte)(105)))));
            this.bunifuButton1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuButton1.BackgroundImage")));
            this.bunifuButton1.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.bunifuButton1.ButtonText = "X";
            this.bunifuButton1.ButtonTextMarginLeft = 0;
            this.bunifuButton1.ColorContrastOnClick = 45;
            this.bunifuButton1.ColorContrastOnHover = 45;
            this.bunifuButton1.Cursor = System.Windows.Forms.Cursors.Default;
            borderEdges1.BottomLeft = true;
            borderEdges1.BottomRight = true;
            borderEdges1.TopLeft = true;
            borderEdges1.TopRight = true;
            this.bunifuButton1.CustomizableEdges = borderEdges1;
            this.bunifuButton1.DialogResult = System.Windows.Forms.DialogResult.None;
            this.bunifuButton1.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.bunifuButton1.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.bunifuButton1.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.bunifuButton1.Dock = System.Windows.Forms.DockStyle.Right;
            this.bunifuButton1.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this.bunifuButton1.Font = new System.Drawing.Font("Arial", 12F);
            this.bunifuButton1.ForeColor = System.Drawing.Color.White;
            this.bunifuButton1.IconLeftAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bunifuButton1.IconLeftCursor = System.Windows.Forms.Cursors.Default;
            this.bunifuButton1.IconLeftPadding = new System.Windows.Forms.Padding(11, 3, 3, 3);
            this.bunifuButton1.IconMarginLeft = 11;
            this.bunifuButton1.IconPadding = 10;
            this.bunifuButton1.IconRightAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bunifuButton1.IconRightCursor = System.Windows.Forms.Cursors.Default;
            this.bunifuButton1.IconRightPadding = new System.Windows.Forms.Padding(3, 3, 7, 3);
            this.bunifuButton1.IconSize = 25;
            this.bunifuButton1.IdleBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(106)))), ((int)(((byte)(105)))));
            this.bunifuButton1.IdleBorderRadius = 1;
            this.bunifuButton1.IdleBorderThickness = 1;
            this.bunifuButton1.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(106)))), ((int)(((byte)(105)))));
            this.bunifuButton1.IdleIconLeftImage = null;
            this.bunifuButton1.IdleIconRightImage = null;
            this.bunifuButton1.IndicateFocus = false;
            this.bunifuButton1.Location = new System.Drawing.Point(565, 0);
            this.bunifuButton1.Name = "bunifuButton1";
            this.bunifuButton1.OnDisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(191)))), ((int)(((byte)(191)))));
            this.bunifuButton1.OnDisabledState.BorderRadius = 1;
            this.bunifuButton1.OnDisabledState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.bunifuButton1.OnDisabledState.BorderThickness = 1;
            this.bunifuButton1.OnDisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.bunifuButton1.OnDisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.bunifuButton1.OnDisabledState.IconLeftImage = null;
            this.bunifuButton1.OnDisabledState.IconRightImage = null;
            this.bunifuButton1.onHoverState.BorderColor = System.Drawing.Color.Red;
            this.bunifuButton1.onHoverState.BorderRadius = 1;
            this.bunifuButton1.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.bunifuButton1.onHoverState.BorderThickness = 1;
            this.bunifuButton1.onHoverState.FillColor = System.Drawing.Color.Red;
            this.bunifuButton1.onHoverState.ForeColor = System.Drawing.Color.White;
            this.bunifuButton1.onHoverState.IconLeftImage = null;
            this.bunifuButton1.onHoverState.IconRightImage = null;
            this.bunifuButton1.OnIdleState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(106)))), ((int)(((byte)(105)))));
            this.bunifuButton1.OnIdleState.BorderRadius = 1;
            this.bunifuButton1.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.bunifuButton1.OnIdleState.BorderThickness = 1;
            this.bunifuButton1.OnIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(106)))), ((int)(((byte)(105)))));
            this.bunifuButton1.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.bunifuButton1.OnIdleState.IconLeftImage = null;
            this.bunifuButton1.OnIdleState.IconRightImage = null;
            this.bunifuButton1.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.bunifuButton1.OnPressedState.BorderRadius = 1;
            this.bunifuButton1.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.bunifuButton1.OnPressedState.BorderThickness = 1;
            this.bunifuButton1.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.bunifuButton1.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.bunifuButton1.OnPressedState.IconLeftImage = null;
            this.bunifuButton1.OnPressedState.IconRightImage = null;
            this.bunifuButton1.Size = new System.Drawing.Size(51, 35);
            this.bunifuButton1.TabIndex = 19;
            this.bunifuButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.bunifuButton1.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.bunifuButton1.TextMarginLeft = 0;
            this.bunifuButton1.TextPadding = new System.Windows.Forms.Padding(0);
            this.bunifuButton1.UseDefaultRadiusAndThickness = true;
            this.bunifuButton1.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // bunifuLabel1
            // 
            this.bunifuLabel1.AllowParentOverrides = false;
            this.bunifuLabel1.AutoEllipsis = false;
            this.bunifuLabel1.Cursor = System.Windows.Forms.Cursors.Default;
            this.bunifuLabel1.CursorType = System.Windows.Forms.Cursors.Default;
            this.bunifuLabel1.Enabled = false;
            this.bunifuLabel1.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.bunifuLabel1.ForeColor = System.Drawing.Color.White;
            this.bunifuLabel1.Location = new System.Drawing.Point(12, 5);
            this.bunifuLabel1.Name = "bunifuLabel1";
            this.bunifuLabel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bunifuLabel1.Size = new System.Drawing.Size(103, 25);
            this.bunifuLabel1.TabIndex = 8;
            this.bunifuLabel1.Text = "Marina Cafe";
            this.bunifuLabel1.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.bunifuLabel1.TextFormat = Bunifu.UI.WinForms.BunifuLabel.TextFormattingOptions.Default;
            // 
            // tb_null
            // 
            this.tb_null.Location = new System.Drawing.Point(90, 9);
            this.tb_null.Name = "tb_null";
            this.tb_null.Size = new System.Drawing.Size(0, 20);
            this.tb_null.TabIndex = 7;
            // 
            // bunifuElipse1
            // 
            this.bunifuElipse1.ElipseRadius = 15;
            this.bunifuElipse1.TargetControl = this;
            // 
            // bunifuElipse2
            // 
            this.bunifuElipse2.ElipseRadius = 15;
            this.bunifuElipse2.TargetControl = this;
            // 
            // bunifuElipse3
            // 
            this.bunifuElipse3.ElipseRadius = 50;
            this.bunifuElipse3.TargetControl = this.btn_session;
            // 
            // btn_session
            // 
            this.btn_session.ActiveImage = null;
            this.btn_session.AllowAnimations = true;
            this.btn_session.AllowBuffering = false;
            this.btn_session.AllowToggling = false;
            this.btn_session.AllowZooming = true;
            this.btn_session.AllowZoomingOnFocus = false;
            this.btn_session.BackColor = System.Drawing.Color.Transparent;
            this.btn_session.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_session.ErrorImage = ((System.Drawing.Image)(resources.GetObject("btn_session.ErrorImage")));
            this.btn_session.FadeWhenInactive = false;
            this.btn_session.Flip = Bunifu.UI.WinForms.BunifuImageButton.FlipOrientation.Normal;
            this.btn_session.ForeColor = System.Drawing.Color.Transparent;
            this.btn_session.Image = global::MarinaCafeProject.Properties.Resources.start_session_soft;
            this.btn_session.ImageActive = null;
            this.btn_session.ImageLocation = null;
            this.btn_session.ImageMargin = 40;
            this.btn_session.ImageSize = new System.Drawing.Size(190, 185);
            this.btn_session.ImageZoomSize = new System.Drawing.Size(230, 225);
            this.btn_session.InitialImage = ((System.Drawing.Image)(resources.GetObject("btn_session.InitialImage")));
            this.btn_session.Location = new System.Drawing.Point(193, 599);
            this.btn_session.Name = "btn_session";
            this.btn_session.Rotation = 0;
            this.btn_session.ShowActiveImage = true;
            this.btn_session.ShowCursorChanges = true;
            this.btn_session.ShowImageBorders = true;
            this.btn_session.ShowSizeMarkers = false;
            this.btn_session.Size = new System.Drawing.Size(230, 225);
            this.btn_session.TabIndex = 19;
            this.btn_session.ToolTipText = "";
            this.btn_session.WaitOnLoad = false;
            this.btn_session.Zoom = 40;
            this.btn_session.ZoomSpeed = 10;
            this.btn_session.Click += new System.EventHandler(this.btn_session_Click);
            // 
            // bunifuImageButton1
            // 
            this.bunifuImageButton1.ActiveImage = null;
            this.bunifuImageButton1.AllowAnimations = true;
            this.bunifuImageButton1.AllowBuffering = false;
            this.bunifuImageButton1.AllowToggling = false;
            this.bunifuImageButton1.AllowZooming = true;
            this.bunifuImageButton1.AllowZoomingOnFocus = false;
            this.bunifuImageButton1.BackColor = System.Drawing.Color.Transparent;
            this.bunifuImageButton1.DialogResult = System.Windows.Forms.DialogResult.None;
            this.bunifuImageButton1.ErrorImage = ((System.Drawing.Image)(resources.GetObject("bunifuImageButton1.ErrorImage")));
            this.bunifuImageButton1.FadeWhenInactive = false;
            this.bunifuImageButton1.Flip = Bunifu.UI.WinForms.BunifuImageButton.FlipOrientation.Normal;
            this.bunifuImageButton1.ForeColor = System.Drawing.Color.Transparent;
            this.bunifuImageButton1.Image = global::MarinaCafeProject.Properties.Resources.info;
            this.bunifuImageButton1.ImageActive = null;
            this.bunifuImageButton1.ImageLocation = null;
            this.bunifuImageButton1.ImageMargin = 10;
            this.bunifuImageButton1.ImageSize = new System.Drawing.Size(23, 23);
            this.bunifuImageButton1.ImageZoomSize = new System.Drawing.Size(33, 33);
            this.bunifuImageButton1.InitialImage = ((System.Drawing.Image)(resources.GetObject("bunifuImageButton1.InitialImage")));
            this.bunifuImageButton1.Location = new System.Drawing.Point(582, 831);
            this.bunifuImageButton1.Name = "bunifuImageButton1";
            this.bunifuImageButton1.Rotation = 0;
            this.bunifuImageButton1.ShowActiveImage = true;
            this.bunifuImageButton1.ShowCursorChanges = true;
            this.bunifuImageButton1.ShowImageBorders = true;
            this.bunifuImageButton1.ShowSizeMarkers = false;
            this.bunifuImageButton1.Size = new System.Drawing.Size(33, 33);
            this.bunifuImageButton1.TabIndex = 19;
            this.bunifuImageButton1.ToolTipText = "";
            this.bunifuImageButton1.WaitOnLoad = false;
            this.bunifuImageButton1.Zoom = 10;
            this.bunifuImageButton1.ZoomSpeed = 10;
            this.bunifuImageButton1.Click += new System.EventHandler(this.bunifuImageButton1_Click);
            // 
            // btn_table_management
            // 
            this.btn_table_management.ActiveImage = null;
            this.btn_table_management.AllowAnimations = true;
            this.btn_table_management.AllowBuffering = false;
            this.btn_table_management.AllowToggling = false;
            this.btn_table_management.AllowZooming = true;
            this.btn_table_management.AllowZoomingOnFocus = false;
            this.btn_table_management.BackColor = System.Drawing.Color.Transparent;
            this.btn_table_management.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_table_management.ErrorImage = ((System.Drawing.Image)(resources.GetObject("btn_table_management.ErrorImage")));
            this.btn_table_management.FadeWhenInactive = false;
            this.btn_table_management.Flip = Bunifu.UI.WinForms.BunifuImageButton.FlipOrientation.Normal;
            this.btn_table_management.ForeColor = System.Drawing.Color.Transparent;
            this.btn_table_management.Image = global::MarinaCafeProject.Properties.Resources.table_management;
            this.btn_table_management.ImageActive = null;
            this.btn_table_management.ImageLocation = null;
            this.btn_table_management.ImageMargin = 40;
            this.btn_table_management.ImageSize = new System.Drawing.Size(140, 140);
            this.btn_table_management.ImageZoomSize = new System.Drawing.Size(180, 180);
            this.btn_table_management.InitialImage = ((System.Drawing.Image)(resources.GetObject("btn_table_management.InitialImage")));
            this.btn_table_management.Location = new System.Drawing.Point(411, 408);
            this.btn_table_management.Name = "btn_table_management";
            this.btn_table_management.Rotation = 0;
            this.btn_table_management.ShowActiveImage = true;
            this.btn_table_management.ShowCursorChanges = true;
            this.btn_table_management.ShowImageBorders = true;
            this.btn_table_management.ShowSizeMarkers = false;
            this.btn_table_management.Size = new System.Drawing.Size(180, 180);
            this.btn_table_management.TabIndex = 19;
            this.btn_table_management.ToolTipText = "";
            this.btn_table_management.WaitOnLoad = false;
            this.btn_table_management.Zoom = 40;
            this.btn_table_management.ZoomSpeed = 10;
            this.btn_table_management.Click += new System.EventHandler(this.btn_sale_history_Click);
            // 
            // btn_sale_history
            // 
            this.btn_sale_history.ActiveImage = null;
            this.btn_sale_history.AllowAnimations = true;
            this.btn_sale_history.AllowBuffering = false;
            this.btn_sale_history.AllowToggling = false;
            this.btn_sale_history.AllowZooming = true;
            this.btn_sale_history.AllowZoomingOnFocus = false;
            this.btn_sale_history.BackColor = System.Drawing.Color.Transparent;
            this.btn_sale_history.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_sale_history.ErrorImage = ((System.Drawing.Image)(resources.GetObject("btn_sale_history.ErrorImage")));
            this.btn_sale_history.FadeWhenInactive = false;
            this.btn_sale_history.Flip = Bunifu.UI.WinForms.BunifuImageButton.FlipOrientation.Normal;
            this.btn_sale_history.ForeColor = System.Drawing.Color.Transparent;
            this.btn_sale_history.Image = global::MarinaCafeProject.Properties.Resources.sale_history;
            this.btn_sale_history.ImageActive = null;
            this.btn_sale_history.ImageLocation = null;
            this.btn_sale_history.ImageMargin = 40;
            this.btn_sale_history.ImageSize = new System.Drawing.Size(140, 140);
            this.btn_sale_history.ImageZoomSize = new System.Drawing.Size(180, 180);
            this.btn_sale_history.InitialImage = ((System.Drawing.Image)(resources.GetObject("btn_sale_history.InitialImage")));
            this.btn_sale_history.Location = new System.Drawing.Point(218, 408);
            this.btn_sale_history.Name = "btn_sale_history";
            this.btn_sale_history.Rotation = 0;
            this.btn_sale_history.ShowActiveImage = true;
            this.btn_sale_history.ShowCursorChanges = true;
            this.btn_sale_history.ShowImageBorders = true;
            this.btn_sale_history.ShowSizeMarkers = false;
            this.btn_sale_history.Size = new System.Drawing.Size(180, 180);
            this.btn_sale_history.TabIndex = 19;
            this.btn_sale_history.ToolTipText = "";
            this.btn_sale_history.WaitOnLoad = false;
            this.btn_sale_history.Zoom = 40;
            this.btn_sale_history.ZoomSpeed = 10;
            this.btn_sale_history.Click += new System.EventHandler(this.btn_sale_history_Click);
            // 
            // btn_product_settings
            // 
            this.btn_product_settings.ActiveImage = null;
            this.btn_product_settings.AllowAnimations = true;
            this.btn_product_settings.AllowBuffering = false;
            this.btn_product_settings.AllowToggling = false;
            this.btn_product_settings.AllowZooming = true;
            this.btn_product_settings.AllowZoomingOnFocus = false;
            this.btn_product_settings.BackColor = System.Drawing.Color.Transparent;
            this.btn_product_settings.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_product_settings.ErrorImage = ((System.Drawing.Image)(resources.GetObject("btn_product_settings.ErrorImage")));
            this.btn_product_settings.FadeWhenInactive = false;
            this.btn_product_settings.Flip = Bunifu.UI.WinForms.BunifuImageButton.FlipOrientation.Normal;
            this.btn_product_settings.ForeColor = System.Drawing.Color.Transparent;
            this.btn_product_settings.Image = global::MarinaCafeProject.Properties.Resources.roasting;
            this.btn_product_settings.ImageActive = null;
            this.btn_product_settings.ImageLocation = null;
            this.btn_product_settings.ImageMargin = 40;
            this.btn_product_settings.ImageSize = new System.Drawing.Size(140, 140);
            this.btn_product_settings.ImageZoomSize = new System.Drawing.Size(180, 180);
            this.btn_product_settings.InitialImage = ((System.Drawing.Image)(resources.GetObject("btn_product_settings.InitialImage")));
            this.btn_product_settings.Location = new System.Drawing.Point(25, 408);
            this.btn_product_settings.Name = "btn_product_settings";
            this.btn_product_settings.Rotation = 0;
            this.btn_product_settings.ShowActiveImage = true;
            this.btn_product_settings.ShowCursorChanges = true;
            this.btn_product_settings.ShowImageBorders = true;
            this.btn_product_settings.ShowSizeMarkers = false;
            this.btn_product_settings.Size = new System.Drawing.Size(180, 180);
            this.btn_product_settings.TabIndex = 19;
            this.btn_product_settings.ToolTipText = "";
            this.btn_product_settings.WaitOnLoad = false;
            this.btn_product_settings.Zoom = 40;
            this.btn_product_settings.ZoomSpeed = 10;
            this.btn_product_settings.Click += new System.EventHandler(this.btn_product_settings_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MarinaCafeProject.Properties.Resources.logo_blue;
            this.pictureBox1.Location = new System.Drawing.Point(120, 50);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(377, 349);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 18;
            this.pictureBox1.TabStop = false;
            // 
            // Launcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(159)))), ((int)(((byte)(157)))));
            this.ClientSize = new System.Drawing.Size(616, 788);
            this.Controls.Add(this.btn_session);
            this.Controls.Add(this.bunifuImageButton1);
            this.Controls.Add(this.btn_table_management);
            this.Controls.Add(this.btn_sale_history);
            this.Controls.Add(this.btn_product_settings);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel_top);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(616, 866);
            this.MinimumSize = new System.Drawing.Size(616, 726);
            this.Name = "Launcher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Launcher_Load);
            this.panel_top.ResumeLayout(false);
            this.panel_top.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuFormDock bunifuFormDock1;
        private Bunifu.Framework.UI.BunifuElipse bunifuElipse1;
        private System.Windows.Forms.Panel panel_top;
        private Bunifu.UI.WinForms.BunifuLabel bunifuLabel1;
        private System.Windows.Forms.TextBox tb_null;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton bunifuButton1;
        private Bunifu.UI.WinForms.BunifuImageButton btn_product_settings;
        private Bunifu.Framework.UI.BunifuElipse bunifuElipse2;
        private Bunifu.UI.WinForms.BunifuImageButton btn_session;
        private Bunifu.Framework.UI.BunifuElipse bunifuElipse3;
        private Bunifu.UI.WinForms.BunifuImageButton btn_sale_history;
        private Bunifu.UI.WinForms.BunifuImageButton bunifuImageButton1;
        private Bunifu.UI.WinForms.BunifuImageButton btn_table_management;
    }
}


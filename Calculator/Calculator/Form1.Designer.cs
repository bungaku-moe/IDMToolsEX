namespace Simple_Windows_Calculator
{
    partial class SimpleCalculator
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleCalculator));
            btnBack = new System.Windows.Forms.Button();
            btnClear = new System.Windows.Forms.Button();
            btnClearEntry = new System.Windows.Forms.Button();
            btnPercent = new System.Windows.Forms.Button();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            btnNum0 = new System.Windows.Forms.Button();
            btnPlusMinus = new System.Windows.Forms.Button();
            btnEqual = new System.Windows.Forms.Button();
            btnDot = new System.Windows.Forms.Button();
            btnNum2 = new System.Windows.Forms.Button();
            btnNum7 = new System.Windows.Forms.Button();
            btnNum3 = new System.Windows.Forms.Button();
            btnAdd = new System.Windows.Forms.Button();
            btnNum6 = new System.Windows.Forms.Button();
            btnNum5 = new System.Windows.Forms.Button();
            btnNum1 = new System.Windows.Forms.Button();
            btnSubtract = new System.Windows.Forms.Button();
            btnNum9 = new System.Windows.Forms.Button();
            btnNum8 = new System.Windows.Forms.Button();
            btnNum4 = new System.Windows.Forms.Button();
            btnMultiply = new System.Windows.Forms.Button();
            btnSquare = new System.Windows.Forms.Button();
            btnInverse = new System.Windows.Forms.Button();
            btnSquareRoot = new System.Windows.Forms.Button();
            btnDivide = new System.Windows.Forms.Button();
            textMainDisplay = new System.Windows.Forms.TextBox();
            textFormulaDisplay = new System.Windows.Forms.TextBox();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // btnBack
            // 
            btnBack.AutoSize = true;
            btnBack.BackColor = System.Drawing.Color.FromArgb(((int)((byte)50)), ((int)((byte)50)), ((int)((byte)50)));
            btnBack.Dock = System.Windows.Forms.DockStyle.Fill;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnBack.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            btnBack.ForeColor = System.Drawing.Color.White;
            btnBack.Location = new System.Drawing.Point(195, 3);
            btnBack.Name = "btnBack";
            btnBack.Size = new System.Drawing.Size(60, 51);
            btnBack.TabIndex = 2;
            btnBack.TabStop = false;
            btnBack.Text = "BACK";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += ButtonBack_Click;
            // 
            // btnClear
            // 
            btnClear.AutoSize = true;
            btnClear.BackColor = System.Drawing.Color.FromArgb(((int)((byte)50)), ((int)((byte)50)), ((int)((byte)50)));
            btnClear.Dock = System.Windows.Forms.DockStyle.Fill;
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnClear.Font = new System.Drawing.Font("Segoe UI Semibold", 14F);
            btnClear.ForeColor = System.Drawing.Color.White;
            btnClear.Location = new System.Drawing.Point(131, 3);
            btnClear.Name = "btnClear";
            btnClear.Size = new System.Drawing.Size(58, 51);
            btnClear.TabIndex = 3;
            btnClear.TabStop = false;
            btnClear.Text = "C";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += ButtonClear_Click;
            // 
            // btnClearEntry
            // 
            btnClearEntry.AutoSize = true;
            btnClearEntry.BackColor = System.Drawing.Color.FromArgb(((int)((byte)50)), ((int)((byte)50)), ((int)((byte)50)));
            btnClearEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            btnClearEntry.FlatAppearance.BorderSize = 0;
            btnClearEntry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnClearEntry.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            btnClearEntry.ForeColor = System.Drawing.Color.White;
            btnClearEntry.Location = new System.Drawing.Point(67, 3);
            btnClearEntry.Name = "btnClearEntry";
            btnClearEntry.Size = new System.Drawing.Size(58, 51);
            btnClearEntry.TabIndex = 1;
            btnClearEntry.TabStop = false;
            btnClearEntry.Text = "CE";
            btnClearEntry.UseVisualStyleBackColor = false;
            btnClearEntry.Click += ButtonClearEntry_Click;
            // 
            // btnPercent
            // 
            btnPercent.AutoSize = true;
            btnPercent.BackColor = System.Drawing.Color.FromArgb(((int)((byte)50)), ((int)((byte)50)), ((int)((byte)50)));
            btnPercent.Dock = System.Windows.Forms.DockStyle.Fill;
            btnPercent.FlatAppearance.BorderSize = 0;
            btnPercent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnPercent.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)0));
            btnPercent.ForeColor = System.Drawing.Color.White;
            btnPercent.Location = new System.Drawing.Point(3, 3);
            btnPercent.Name = "btnPercent";
            btnPercent.Size = new System.Drawing.Size(58, 51);
            btnPercent.TabIndex = 0;
            btnPercent.TabStop = false;
            btnPercent.Text = "%";
            btnPercent.UseVisualStyleBackColor = false;
            btnPercent.Click += ButtonSpecialOperation_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel1.Controls.Add(btnNum0, 0, 5);
            tableLayoutPanel1.Controls.Add(btnPlusMinus, 0, 5);
            tableLayoutPanel1.Controls.Add(btnEqual, 0, 5);
            tableLayoutPanel1.Controls.Add(btnDot, 0, 5);
            tableLayoutPanel1.Controls.Add(btnNum2, 0, 4);
            tableLayoutPanel1.Controls.Add(btnNum7, 0, 1);
            tableLayoutPanel1.Controls.Add(btnNum3, 0, 4);
            tableLayoutPanel1.Controls.Add(btnAdd, 0, 4);
            tableLayoutPanel1.Controls.Add(btnNum6, 0, 3);
            tableLayoutPanel1.Controls.Add(btnNum5, 0, 3);
            tableLayoutPanel1.Controls.Add(btnNum1, 0, 3);
            tableLayoutPanel1.Controls.Add(btnSubtract, 0, 3);
            tableLayoutPanel1.Controls.Add(btnNum9, 0, 2);
            tableLayoutPanel1.Controls.Add(btnNum8, 0, 2);
            tableLayoutPanel1.Controls.Add(btnNum4, 0, 2);
            tableLayoutPanel1.Controls.Add(btnMultiply, 0, 2);
            tableLayoutPanel1.Controls.Add(btnSquare, 0, 1);
            tableLayoutPanel1.Controls.Add(btnInverse, 0, 1);
            tableLayoutPanel1.Controls.Add(btnSquareRoot, 0, 1);
            tableLayoutPanel1.Controls.Add(btnDivide, 0, 1);
            tableLayoutPanel1.Controls.Add(btnPercent, 0, 0);
            tableLayoutPanel1.Controls.Add(btnClearEntry, 1, 0);
            tableLayoutPanel1.Controls.Add(btnClear, 2, 0);
            tableLayoutPanel1.Controls.Add(btnBack, 3, 0);
            tableLayoutPanel1.Location = new System.Drawing.Point(3, 150);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.666672F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.666668F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.666668F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.666668F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.666668F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.666668F));
            tableLayoutPanel1.Size = new System.Drawing.Size(258, 343);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // btnNum0
            // 
            btnNum0.AutoSize = true;
            btnNum0.BackColor = System.Drawing.Color.FromArgb(((int)((byte)59)), ((int)((byte)59)), ((int)((byte)59)));
            btnNum0.Dock = System.Windows.Forms.DockStyle.Fill;
            btnNum0.FlatAppearance.BorderSize = 0;
            btnNum0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnNum0.Font = new System.Drawing.Font("Segoe UI Semibold", 16F);
            btnNum0.ForeColor = System.Drawing.Color.White;
            btnNum0.Location = new System.Drawing.Point(67, 288);
            btnNum0.Name = "btnNum0";
            btnNum0.Size = new System.Drawing.Size(58, 52);
            btnNum0.TabIndex = 28;
            btnNum0.TabStop = false;
            btnNum0.Text = "0";
            btnNum0.UseVisualStyleBackColor = false;
            btnNum0.Click += ButtonNumbers_Click;
            // 
            // btnPlusMinus
            // 
            btnPlusMinus.AutoSize = true;
            btnPlusMinus.BackColor = System.Drawing.Color.FromArgb(((int)((byte)59)), ((int)((byte)59)), ((int)((byte)59)));
            btnPlusMinus.Dock = System.Windows.Forms.DockStyle.Fill;
            btnPlusMinus.FlatAppearance.BorderSize = 0;
            btnPlusMinus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnPlusMinus.Font = new System.Drawing.Font("Segoe UI Semibold", 16F);
            btnPlusMinus.ForeColor = System.Drawing.Color.White;
            btnPlusMinus.Location = new System.Drawing.Point(3, 288);
            btnPlusMinus.Name = "btnPlusMinus";
            btnPlusMinus.Size = new System.Drawing.Size(58, 52);
            btnPlusMinus.TabIndex = 27;
            btnPlusMinus.TabStop = false;
            btnPlusMinus.Text = "+/-";
            btnPlusMinus.UseVisualStyleBackColor = false;
            btnPlusMinus.Click += ButtonPlusMinus_Click;
            // 
            // btnEqual
            // 
            btnEqual.AutoSize = true;
            btnEqual.BackColor = System.Drawing.Color.FromArgb(((int)((byte)76)), ((int)((byte)194)), ((int)((byte)255)));
            btnEqual.Dock = System.Windows.Forms.DockStyle.Fill;
            btnEqual.FlatAppearance.BorderSize = 0;
            btnEqual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnEqual.Font = new System.Drawing.Font("Segoe UI Semibold", 18F);
            btnEqual.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)21)), ((int)((byte)54)), ((int)((byte)71)));
            btnEqual.Location = new System.Drawing.Point(195, 288);
            btnEqual.Name = "btnEqual";
            btnEqual.Size = new System.Drawing.Size(60, 52);
            btnEqual.TabIndex = 26;
            btnEqual.TabStop = false;
            btnEqual.Text = "=";
            btnEqual.UseVisualStyleBackColor = false;
            btnEqual.Click += ButtonEqual_Click;
            // 
            // btnDot
            // 
            btnDot.AutoSize = true;
            btnDot.BackColor = System.Drawing.Color.FromArgb(((int)((byte)59)), ((int)((byte)59)), ((int)((byte)59)));
            btnDot.Dock = System.Windows.Forms.DockStyle.Fill;
            btnDot.FlatAppearance.BorderSize = 0;
            btnDot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnDot.Font = new System.Drawing.Font("Segoe UI Semibold", 16F);
            btnDot.ForeColor = System.Drawing.Color.White;
            btnDot.Location = new System.Drawing.Point(131, 288);
            btnDot.Name = "btnDot";
            btnDot.Size = new System.Drawing.Size(58, 52);
            btnDot.TabIndex = 25;
            btnDot.TabStop = false;
            btnDot.Text = ".";
            btnDot.UseVisualStyleBackColor = false;
            btnDot.Click += ButtonDot_Click;
            // 
            // btnNum2
            // 
            btnNum2.AutoSize = true;
            btnNum2.BackColor = System.Drawing.Color.FromArgb(((int)((byte)59)), ((int)((byte)59)), ((int)((byte)59)));
            btnNum2.Dock = System.Windows.Forms.DockStyle.Fill;
            btnNum2.FlatAppearance.BorderSize = 0;
            btnNum2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnNum2.Font = new System.Drawing.Font("Segoe UI Semibold", 16F);
            btnNum2.ForeColor = System.Drawing.Color.White;
            btnNum2.Location = new System.Drawing.Point(67, 231);
            btnNum2.Name = "btnNum2";
            btnNum2.Size = new System.Drawing.Size(58, 51);
            btnNum2.TabIndex = 24;
            btnNum2.TabStop = false;
            btnNum2.Text = "2";
            btnNum2.UseVisualStyleBackColor = false;
            btnNum2.Click += ButtonNumbers_Click;
            // 
            // btnNum7
            // 
            btnNum7.AutoSize = true;
            btnNum7.BackColor = System.Drawing.Color.FromArgb(((int)((byte)59)), ((int)((byte)59)), ((int)((byte)59)));
            btnNum7.Dock = System.Windows.Forms.DockStyle.Fill;
            btnNum7.FlatAppearance.BorderSize = 0;
            btnNum7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnNum7.Font = new System.Drawing.Font("Segoe UI Semibold", 16F);
            btnNum7.ForeColor = System.Drawing.Color.White;
            btnNum7.Location = new System.Drawing.Point(3, 117);
            btnNum7.Name = "btnNum7";
            btnNum7.Size = new System.Drawing.Size(58, 51);
            btnNum7.TabIndex = 23;
            btnNum7.TabStop = false;
            btnNum7.Text = "7";
            btnNum7.UseVisualStyleBackColor = false;
            btnNum7.Click += ButtonNumbers_Click;
            // 
            // btnNum3
            // 
            btnNum3.AutoSize = true;
            btnNum3.BackColor = System.Drawing.Color.FromArgb(((int)((byte)59)), ((int)((byte)59)), ((int)((byte)59)));
            btnNum3.Dock = System.Windows.Forms.DockStyle.Fill;
            btnNum3.FlatAppearance.BorderSize = 0;
            btnNum3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnNum3.Font = new System.Drawing.Font("Segoe UI Semibold", 16F);
            btnNum3.ForeColor = System.Drawing.Color.White;
            btnNum3.Location = new System.Drawing.Point(131, 231);
            btnNum3.Name = "btnNum3";
            btnNum3.Size = new System.Drawing.Size(58, 51);
            btnNum3.TabIndex = 22;
            btnNum3.TabStop = false;
            btnNum3.Text = "3";
            btnNum3.UseVisualStyleBackColor = false;
            btnNum3.Click += ButtonNumbers_Click;
            // 
            // btnAdd
            // 
            btnAdd.AutoSize = true;
            btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)((byte)50)), ((int)((byte)50)), ((int)((byte)50)));
            btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAdd.Font = new System.Drawing.Font("Segoe UI Semibold", 18F);
            btnAdd.ForeColor = System.Drawing.Color.White;
            btnAdd.Location = new System.Drawing.Point(195, 231);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(60, 51);
            btnAdd.TabIndex = 21;
            btnAdd.TabStop = false;
            btnAdd.Text = "+";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += ButtonBasicOperation_Click;
            // 
            // btnNum6
            // 
            btnNum6.AutoSize = true;
            btnNum6.BackColor = System.Drawing.Color.FromArgb(((int)((byte)59)), ((int)((byte)59)), ((int)((byte)59)));
            btnNum6.Dock = System.Windows.Forms.DockStyle.Fill;
            btnNum6.FlatAppearance.BorderSize = 0;
            btnNum6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnNum6.Font = new System.Drawing.Font("Segoe UI Semibold", 16F);
            btnNum6.ForeColor = System.Drawing.Color.White;
            btnNum6.Location = new System.Drawing.Point(131, 174);
            btnNum6.Name = "btnNum6";
            btnNum6.Size = new System.Drawing.Size(58, 51);
            btnNum6.TabIndex = 20;
            btnNum6.TabStop = false;
            btnNum6.Text = "6";
            btnNum6.UseVisualStyleBackColor = false;
            btnNum6.Click += ButtonNumbers_Click;
            // 
            // btnNum5
            // 
            btnNum5.AutoSize = true;
            btnNum5.BackColor = System.Drawing.Color.FromArgb(((int)((byte)59)), ((int)((byte)59)), ((int)((byte)59)));
            btnNum5.Dock = System.Windows.Forms.DockStyle.Fill;
            btnNum5.FlatAppearance.BorderSize = 0;
            btnNum5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnNum5.Font = new System.Drawing.Font("Segoe UI Semibold", 16F);
            btnNum5.ForeColor = System.Drawing.Color.White;
            btnNum5.Location = new System.Drawing.Point(67, 174);
            btnNum5.Name = "btnNum5";
            btnNum5.Size = new System.Drawing.Size(58, 51);
            btnNum5.TabIndex = 19;
            btnNum5.TabStop = false;
            btnNum5.Text = "5";
            btnNum5.UseVisualStyleBackColor = false;
            btnNum5.Click += ButtonNumbers_Click;
            // 
            // btnNum1
            // 
            btnNum1.AutoSize = true;
            btnNum1.BackColor = System.Drawing.Color.FromArgb(((int)((byte)59)), ((int)((byte)59)), ((int)((byte)59)));
            btnNum1.Dock = System.Windows.Forms.DockStyle.Fill;
            btnNum1.FlatAppearance.BorderSize = 0;
            btnNum1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnNum1.Font = new System.Drawing.Font("Segoe UI Semibold", 16F);
            btnNum1.ForeColor = System.Drawing.Color.White;
            btnNum1.Location = new System.Drawing.Point(3, 231);
            btnNum1.Name = "btnNum1";
            btnNum1.Size = new System.Drawing.Size(58, 51);
            btnNum1.TabIndex = 18;
            btnNum1.TabStop = false;
            btnNum1.Text = "1";
            btnNum1.UseVisualStyleBackColor = false;
            btnNum1.Click += ButtonNumbers_Click;
            // 
            // btnSubtract
            // 
            btnSubtract.AutoSize = true;
            btnSubtract.BackColor = System.Drawing.Color.FromArgb(((int)((byte)50)), ((int)((byte)50)), ((int)((byte)50)));
            btnSubtract.Dock = System.Windows.Forms.DockStyle.Fill;
            btnSubtract.FlatAppearance.BorderSize = 0;
            btnSubtract.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSubtract.Font = new System.Drawing.Font("Segoe UI Semibold", 18F);
            btnSubtract.ForeColor = System.Drawing.Color.White;
            btnSubtract.Location = new System.Drawing.Point(195, 174);
            btnSubtract.Name = "btnSubtract";
            btnSubtract.Size = new System.Drawing.Size(60, 51);
            btnSubtract.TabIndex = 17;
            btnSubtract.TabStop = false;
            btnSubtract.Text = "-";
            btnSubtract.UseVisualStyleBackColor = false;
            btnSubtract.Click += ButtonBasicOperation_Click;
            // 
            // btnNum9
            // 
            btnNum9.AutoSize = true;
            btnNum9.BackColor = System.Drawing.Color.FromArgb(((int)((byte)59)), ((int)((byte)59)), ((int)((byte)59)));
            btnNum9.Dock = System.Windows.Forms.DockStyle.Fill;
            btnNum9.FlatAppearance.BorderSize = 0;
            btnNum9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnNum9.Font = new System.Drawing.Font("Segoe UI Semibold", 16F);
            btnNum9.ForeColor = System.Drawing.Color.White;
            btnNum9.Location = new System.Drawing.Point(131, 117);
            btnNum9.Name = "btnNum9";
            btnNum9.Size = new System.Drawing.Size(58, 51);
            btnNum9.TabIndex = 16;
            btnNum9.TabStop = false;
            btnNum9.Text = "9";
            btnNum9.UseVisualStyleBackColor = false;
            btnNum9.Click += ButtonNumbers_Click;
            // 
            // btnNum8
            // 
            btnNum8.AutoSize = true;
            btnNum8.BackColor = System.Drawing.Color.FromArgb(((int)((byte)59)), ((int)((byte)59)), ((int)((byte)59)));
            btnNum8.Dock = System.Windows.Forms.DockStyle.Fill;
            btnNum8.FlatAppearance.BorderSize = 0;
            btnNum8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnNum8.Font = new System.Drawing.Font("Segoe UI Semibold", 16F);
            btnNum8.ForeColor = System.Drawing.Color.White;
            btnNum8.Location = new System.Drawing.Point(67, 117);
            btnNum8.Name = "btnNum8";
            btnNum8.Size = new System.Drawing.Size(58, 51);
            btnNum8.TabIndex = 15;
            btnNum8.TabStop = false;
            btnNum8.Text = "8";
            btnNum8.UseVisualStyleBackColor = false;
            btnNum8.Click += ButtonNumbers_Click;
            // 
            // btnNum4
            // 
            btnNum4.AutoSize = true;
            btnNum4.BackColor = System.Drawing.Color.FromArgb(((int)((byte)59)), ((int)((byte)59)), ((int)((byte)59)));
            btnNum4.Dock = System.Windows.Forms.DockStyle.Fill;
            btnNum4.FlatAppearance.BorderSize = 0;
            btnNum4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnNum4.Font = new System.Drawing.Font("Segoe UI Semibold", 16F);
            btnNum4.ForeColor = System.Drawing.Color.White;
            btnNum4.Location = new System.Drawing.Point(3, 174);
            btnNum4.Name = "btnNum4";
            btnNum4.Size = new System.Drawing.Size(58, 51);
            btnNum4.TabIndex = 14;
            btnNum4.TabStop = false;
            btnNum4.Text = "4";
            btnNum4.UseVisualStyleBackColor = false;
            btnNum4.Click += ButtonNumbers_Click;
            // 
            // btnMultiply
            // 
            btnMultiply.AutoSize = true;
            btnMultiply.BackColor = System.Drawing.Color.FromArgb(((int)((byte)50)), ((int)((byte)50)), ((int)((byte)50)));
            btnMultiply.Dock = System.Windows.Forms.DockStyle.Fill;
            btnMultiply.FlatAppearance.BorderSize = 0;
            btnMultiply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnMultiply.Font = new System.Drawing.Font("Segoe UI Semibold", 18F);
            btnMultiply.ForeColor = System.Drawing.Color.White;
            btnMultiply.Location = new System.Drawing.Point(195, 117);
            btnMultiply.Name = "btnMultiply";
            btnMultiply.Size = new System.Drawing.Size(60, 51);
            btnMultiply.TabIndex = 13;
            btnMultiply.TabStop = false;
            btnMultiply.Text = "×";
            btnMultiply.UseVisualStyleBackColor = false;
            btnMultiply.Click += ButtonBasicOperation_Click;
            // 
            // btnSquare
            // 
            btnSquare.AutoSize = true;
            btnSquare.BackColor = System.Drawing.Color.FromArgb(((int)((byte)50)), ((int)((byte)50)), ((int)((byte)50)));
            btnSquare.Dock = System.Windows.Forms.DockStyle.Fill;
            btnSquare.FlatAppearance.BorderSize = 0;
            btnSquare.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSquare.Font = new System.Drawing.Font("Segoe UI Semibold", 14F);
            btnSquare.ForeColor = System.Drawing.Color.White;
            btnSquare.Location = new System.Drawing.Point(67, 60);
            btnSquare.Name = "btnSquare";
            btnSquare.Size = new System.Drawing.Size(58, 51);
            btnSquare.TabIndex = 12;
            btnSquare.TabStop = false;
            btnSquare.Text = "x²";
            btnSquare.UseVisualStyleBackColor = false;
            btnSquare.Click += ButtonSpecialOperation_Click;
            // 
            // btnInverse
            // 
            btnInverse.AutoSize = true;
            btnInverse.BackColor = System.Drawing.Color.FromArgb(((int)((byte)50)), ((int)((byte)50)), ((int)((byte)50)));
            btnInverse.Dock = System.Windows.Forms.DockStyle.Fill;
            btnInverse.FlatAppearance.BorderSize = 0;
            btnInverse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnInverse.Font = new System.Drawing.Font("Segoe UI Semibold", 14F);
            btnInverse.ForeColor = System.Drawing.Color.White;
            btnInverse.Location = new System.Drawing.Point(3, 60);
            btnInverse.Name = "btnInverse";
            btnInverse.Size = new System.Drawing.Size(58, 51);
            btnInverse.TabIndex = 11;
            btnInverse.TabStop = false;
            btnInverse.Text = "1/x";
            btnInverse.UseVisualStyleBackColor = false;
            btnInverse.Click += ButtonSpecialOperation_Click;
            // 
            // btnSquareRoot
            // 
            btnSquareRoot.AutoSize = true;
            btnSquareRoot.BackColor = System.Drawing.Color.FromArgb(((int)((byte)50)), ((int)((byte)50)), ((int)((byte)50)));
            btnSquareRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            btnSquareRoot.FlatAppearance.BorderSize = 0;
            btnSquareRoot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSquareRoot.Font = new System.Drawing.Font("Segoe UI Semibold", 14F);
            btnSquareRoot.ForeColor = System.Drawing.Color.White;
            btnSquareRoot.Location = new System.Drawing.Point(131, 60);
            btnSquareRoot.Name = "btnSquareRoot";
            btnSquareRoot.Size = new System.Drawing.Size(58, 51);
            btnSquareRoot.TabIndex = 10;
            btnSquareRoot.TabStop = false;
            btnSquareRoot.Text = "√x";
            btnSquareRoot.UseVisualStyleBackColor = false;
            btnSquareRoot.Click += ButtonSpecialOperation_Click;
            // 
            // btnDivide
            // 
            btnDivide.AutoSize = true;
            btnDivide.BackColor = System.Drawing.Color.FromArgb(((int)((byte)50)), ((int)((byte)50)), ((int)((byte)50)));
            btnDivide.Dock = System.Windows.Forms.DockStyle.Fill;
            btnDivide.FlatAppearance.BorderSize = 0;
            btnDivide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnDivide.Font = new System.Drawing.Font("Segoe UI Semibold", 18F);
            btnDivide.ForeColor = System.Drawing.Color.White;
            btnDivide.Location = new System.Drawing.Point(195, 60);
            btnDivide.Name = "btnDivide";
            btnDivide.Size = new System.Drawing.Size(60, 51);
            btnDivide.TabIndex = 9;
            btnDivide.TabStop = false;
            btnDivide.Text = "÷";
            btnDivide.UseVisualStyleBackColor = false;
            btnDivide.Click += ButtonBasicOperation_Click;
            // 
            // textMainDisplay
            // 
            textMainDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
            textMainDisplay.BackColor = System.Drawing.Color.FromArgb(((int)((byte)32)), ((int)((byte)32)), ((int)((byte)32)));
            textMainDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textMainDisplay.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Bold);
            textMainDisplay.ForeColor = System.Drawing.Color.White;
            textMainDisplay.Location = new System.Drawing.Point(12, 71);
            textMainDisplay.Margin = new System.Windows.Forms.Padding(12);
            textMainDisplay.Name = "textMainDisplay";
            textMainDisplay.Size = new System.Drawing.Size(240, 64);
            textMainDisplay.TabIndex = 1;
            textMainDisplay.TabStop = false;
            textMainDisplay.Text = "0";
            textMainDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            textMainDisplay.MouseDown += MainDisplay_MouseDown;
            // 
            // textFormulaDisplay
            // 
            textFormulaDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
            textFormulaDisplay.BackColor = System.Drawing.Color.FromArgb(((int)((byte)32)), ((int)((byte)32)), ((int)((byte)32)));
            textFormulaDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textFormulaDisplay.Enabled = false;
            textFormulaDisplay.Font = new System.Drawing.Font("Segoe UI Semibold", 24F, System.Drawing.FontStyle.Bold);
            textFormulaDisplay.ForeColor = System.Drawing.Color.White;
            textFormulaDisplay.Location = new System.Drawing.Point(15, 26);
            textFormulaDisplay.Margin = new System.Windows.Forms.Padding(5);
            textFormulaDisplay.Name = "textFormulaDisplay";
            textFormulaDisplay.ReadOnly = true;
            textFormulaDisplay.Size = new System.Drawing.Size(237, 43);
            textFormulaDisplay.TabIndex = 2;
            textFormulaDisplay.TabStop = false;
            textFormulaDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(tableLayoutPanel1, 0, 1);
            tableLayoutPanel2.Controls.Add(textMainDisplay, 0, 0);
            tableLayoutPanel2.Location = new System.Drawing.Point(1, -1);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 29.637096F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70.3629F));
            tableLayoutPanel2.Size = new System.Drawing.Size(264, 496);
            tableLayoutPanel2.TabIndex = 3;
            // 
            // SimpleCalculator
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(((int)((byte)32)), ((int)((byte)32)), ((int)((byte)32)));
            ClientSize = new System.Drawing.Size(264, 495);
            Controls.Add(textFormulaDisplay);
            Controls.Add(tableLayoutPanel2);
            Icon = ((System.Drawing.Icon)resources.GetObject("$this.Icon"));
            KeyPreview = true;
            MaximizeBox = false;
            MinimumSize = new System.Drawing.Size(280, 534);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Calculator";
            Load += Form1_Load;
            KeyDown += SimpleCalculator_KeyDown;
            KeyPress += SimpleCalculator_KeyPress;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;

        private void BtnNum4_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnNum7_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnClearEntry;
        private System.Windows.Forms.Button btnPercent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnNum0;
        private System.Windows.Forms.Button btnPlusMinus;
        private System.Windows.Forms.Button btnEqual;
        private System.Windows.Forms.Button btnDot;
        private System.Windows.Forms.Button btnNum2;
        private System.Windows.Forms.Button btnNum7;
        private System.Windows.Forms.Button btnNum3;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnNum6;
        private System.Windows.Forms.Button btnNum5;
        private System.Windows.Forms.Button btnNum1;
        private System.Windows.Forms.Button btnSubtract;
        private System.Windows.Forms.Button btnNum9;
        private System.Windows.Forms.Button btnNum8;
        private System.Windows.Forms.Button btnNum4;
        private System.Windows.Forms.Button btnMultiply;
        private System.Windows.Forms.Button btnSquare;
        private System.Windows.Forms.Button btnInverse;
        private System.Windows.Forms.Button btnSquareRoot;
        private System.Windows.Forms.Button btnDivide;
        private System.Windows.Forms.TextBox textMainDisplay;
        private System.Windows.Forms.TextBox textFormulaDisplay;
    }
}

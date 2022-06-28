﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assignment1.BusinessObject;
using Assignment1.DataAccess;
using Assignment1.Repository;


namespace MyStoreWinApp
{
    public partial class FrmMemberManagement : Form
    {
        public FrmMemberManagement()
        {
            InitializeComponent();
        }

        IMemberRepository memberRepository = new MemberRepository();
        BindingSource source;

        private void FrmMemberManagement_Load(object sender, EventArgs e)
        {

        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadMemberList();
        }
        private void dgvMemberList_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FrmMemberDetail frmMemberDetail = new FrmMemberDetail
            {
                Text = "Add new Member",
                MemberRepository = memberRepository
            };
            if (frmMemberDetail.ShowDialog() == DialogResult.OK)
            {
                LoadMemberList();
                source.Position = source.Count - 1;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                var member = new MemberObject
                {
                    ID = int.Parse(txtID.Text),
                    Name = txtName.Text,
                    City = txtCity.Text,
                    Country = txtCountry.Text,
                    Email = txtEmail.Text,
                    Password = txtPassword.Text
                };
                FrmConfirm frmConfirm = new FrmConfirm();
                if (frmConfirm.ShowDialog(this) == DialogResult.OK)
                {
                    memberRepository.UpdateMember(member);
                }
                //LoadMemberList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Update fail");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private MemberObject GetMemberObject()
        {
            MemberObject member = null;
            try
            {
                member = new MemberObject
                {
                    ID = int.Parse(txtID.Text),
                    Name = txtName.Text,
                    Country = txtCountry.Text,
                    City = txtCity.Text,
                    Email = txtEmail.Text,
                    Password = txtPassword.Text
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Get Member");
            }
            return member;
        }

        private void LoadMemberList()
        {
            var member = memberRepository.GetMembers();
            try
            {
                source = new BindingSource();
                source.DataSource = member;

                txtID.DataBindings.Clear();
                txtName.DataBindings.Clear();
                txtCountry.DataBindings.Clear();
                txtCity.DataBindings.Clear();
                txtEmail.DataBindings.Clear();
                txtPassword.DataBindings.Clear();

                txtID.DataBindings.Add("Text", source, "ID");
                txtName.DataBindings.Add("Text", source, "Name");
                txtCountry.DataBindings.Add("Text", source, "Country");
                txtCity.DataBindings.Add("Text", source, "City");
                txtEmail.DataBindings.Add("Text", source, "Email");
                txtPassword.DataBindings.Add("Text", source, "Password");

                dgvMemberList.DataSource = null;
                dgvMemberList.DataSource = source;
                if (member.Count() == 0)
                {
                    ClearText();
                    btnDelete.Enabled = false;
                }
                else
                {
                    btnDelete.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load Member List");
            }
        }
        private void ClearText()
        {
            txtID.Text = string.Empty;
            txtName.Text = string.Empty;
            txtCountry.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPassword.Text = string.Empty;

        }

    }
}

/****************************** Module Header ******************************\
* Module Name:	MFCDialogDlg.h
* Project:		MFCDialog
* Copyright (c) Microsoft Corporation.
* 
* The MFCDialog example demonstrates the creation of modal and modeless 
* dialog boxes in MFC.
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
* All other rights reserved.
* 
* History:
* * 1/21/2009 11:04 PM Jialiang Ge Created
\***************************************************************************/

#pragma once


// CMFCDialogDlg dialog
class CMFCDialogDlg : public CDialog
{
// Construction
public:
	CMFCDialogDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	enum { IDD = IDD_MFCDIALOG_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedShowModalDialog();
	afx_msg void OnBnClickedShowModelessDialog();
};

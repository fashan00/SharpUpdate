############################################################################################
#      NSIS Installation Script created by NSIS Quick Setup Script Generator v1.09.18
#               Entirely Edited with NullSoft Scriptable Installation System                
#              by Vlasis K. Barkas aka Red Wine red_wine@freemail.gr Sep 2006               
############################################################################################

#### 指令網站　http://omega.idv.tw/nsis/Contents.html ####

!include "LogicLib.nsh"
!include "MUI.nsh"
!include "DotNetVer.nsh"
!include "fileassoc.nsh"

!define APP_NAME "SimMAGIC QB"
!define APP_ConfigProductVersion "SimMAGIC QB"
!define COMP_NAME "HamaStar Technology Co., Ltd."
!define WEB_SITE "http://www.simmagic.com.tw/"
!define VERSION "4.0.1.13"
!define SilverlightVERSION "2015.6.15.1844"
!define COPYRIGHT "Copyright (C) HamaStar Technology Co., Ltd. 2011"
!define DESCRIPTION "SimMAGIC Application"
!define INSTALL_TYPE "SetShellVarContext current"
!define REG_ROOT "HKCU"
!define UNINSTALL_PATH "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_ConfigProductVersion}"

######################################################################

# 為安裝檔(.exe)增加版本資訊(安裝檔右鍵內容詳細資料)
VIProductVersion "${SilverlightVERSION}" 			# 檔案版本 (Silverlight版本)
VIAddVersionKey "FileVersion" ${SilverlightVERSION} # 因NSIS寫入檔案版本無效，所以使用上一行VIProductVersion
VIAddVersionKey "ProductVersion" ${VERSION} 		# 產品版本 (Clickonce版本)
VIAddVersionKey "ProductName" "${APP_NAME}"
VIAddVersionKey "CompanyName" "${COMP_NAME}"
VIAddVersionKey "LegalCopyright" "${COPYRIGHT}"
VIAddVersionKey "FileDescription" "${DESCRIPTION}"

######################################################################

# 安裝檔基本設定
RequestExecutionLevel admin
SetCompressor ZLIB
Name "${APP_NAME}"
Caption "${APP_NAME}"
OutFile "${APP_NAME}.exe"
BrandingText "${APP_NAME}"
XPStyle on
InstallDirRegKey "${REG_ROOT}" "${UNINSTALL_PATH}" "AppPath"
InstallDir "$PROGRAMFILES\HamaStar\${APP_ConfigProductVersion}"
SpaceTexts none
!addplugindir "."

######################################################################

# 介面(Page)設定
!define MUI_ABORTWARNING
!define MUI_UNABORTWARNING

!define MUI_LANGDLL_REGISTRY_ROOT "${REG_ROOT}"
!define MUI_LANGDLL_REGISTRY_KEY "${UNINSTALL_PATH}"
!define MUI_LANGDLL_REGISTRY_VALUENAME "Installer Language"

# 安裝介面圖、Icon圖
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP "nsis-2.46.5-Unicode\Contrib\Graphics\Header\orange.bmp"
!define MUI_HEADERIMAGE_UNBITMAP "nsis-2.46.5-Unicode\Contrib\Graphics\Header\orange-uninstall.bmp"

!define MUI_WELCOMEFINISHPAGE_BITMAP "nsis-2.46.5-Unicode\Contrib\Graphics\Wizard\orange.bmp"
!define MUI_UNWELCOMEFINISHPAGE_BITMAP "nsis-2.46.5-Unicode\Contrib\Graphics\Wizard\orange-uninstall.bmp"

!define MUI_ICON "nsis-2.46.5-Unicode\Contrib\Graphics\Icons\orange-install.ico"
!define MUI_UNICON "nsis-2.46.5-Unicode\Contrib\Graphics\Icons\orange-uninstall.ico"

# 開始安裝頁面
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!define MUI_FINISHPAGE_RUN "$INSTDIR\SimMAGIC eBook.exe" #安裝完成執行SimMAGIC eBook.exe
!insertmacro MUI_PAGE_FINISH

# 解除安裝頁面
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

# 多語系選擇介面
!insertmacro MUI_RESERVEFILE_LANGDLL
!insertmacro MUI_LANGUAGE "TradChinese"
#!insertmacro MUI_LANGUAGE "SimpChinese"
#!insertmacro MUI_LANGUAGE "English"
#!insertmacro MUI_LANGUAGE "Japanese"
#!insertmacro MUI_LANGUAGE "Italian"
#!insertmacro MUI_LANGUAGE "Malay"

# 宣告多語系對應的機碼值
LangString GlobalLang ${LANG_TradChinese} "zh-TW"
#LangString GlobalLang ${LANG_SimpChinese} "zh-CHS"
#LangString GlobalLang ${LANG_English} "en"
#LangString GlobalLang ${LANG_Japanese} "ja"
#LangString GlobalLang ${LANG_Italian} "it"
#LangString GlobalLang ${LANG_Malay} "ms"

######################################################################

Function .onInit

# +2 means that next instruction is skipped if IfSilent is false. 0 means that compiler should go to next instruction if IfSilent is true.
IfSilent 0 +2
  SetSilent silent
	
!insertmacro MUI_LANGDLL_DISPLAY


FunctionEnd

######################################################################

# Clickonce
Section MainProgram
${INSTALL_TYPE}
#SetOverwrite ifnewer
SetOutPath "$INSTDIR"

# 使用Nsis7z plug-in解壓縮
File "SimMAGIC.7z"
# 包含解壓進度
Nsis7z::ExtractWithDetails "SimMAGIC.7z" "Installing package %s..."
Delete "SimMAGIC.7z"

# 雜項設定(Config, Copyright, 浮水印, 捷徑圖)
File "Basic Settings\Config.ini"
File "Basic Settings\Copyright.rtf"
File "Basic Settings\CopyrightReg.rtf"
File "Basic Settings\watermark.xml"
File "Basic Settings\Shortcut.ico"

# Book母版範本
ExecWait "$INSTDIR\BookTemplates.exe"
Delete "$INSTDIR\BookTemplates.exe"

# ckeditor.exe
ExecWait "$INSTDIR\ckeditor.exe"
Delete "$INSTDIR\ckeditor.exe"

# 檔案關聯
# 參數: 副檔名, FILECLASS(HKEY_CLASSES_ROOT中的檔案關聯設定), 檔案說明(右鍵->內容->檔案類型), 圖示, 自訂右鍵菜單說明, 執行檔 
!insertmacro APP_ASSOCIATE "sd" "SimMAGIC.sd" "EBook SD" "$INSTDIR\Shortcut.ico" "" "$INSTDIR\SimMAGIC eBook.exe $\"%1$\""
!insertmacro UPDATEFILEASSOC
 

SectionEnd

######################################################################

##  需要的軟體
!define SilverlightInstaller "Silverlight4.exe"
!define NETInstaller "dotNetFx40_Full_x86_x64.exe"

Section InstallRequired
  
IfSilent 0 +2
  Goto done
  
  File /oname=$TEMP\${SilverlightInstaller} "required\${SilverlightInstaller}"  
  DetailPrint "Starting Silverlight Setup..."
  ExecWait "$TEMP\${SilverlightInstaller} /q"
  DetailPrint "Silverlight is already installed!"
  
${If} ${HasDotNet4.0}
  DetailPrint "Microsoft .NET Framework is already installed!"
${Else}
  File /oname=$TEMP\${NETInstaller} "required\${NETInstaller}" 
  DetailPrint "Starting Microsoft .NET Framework v4 Setup..."
  ExecWait "$TEMP\${NETInstaller} /passive /norestart"
${EndIf}
  
  done:
SectionEnd

######################################################################

# 捷徑、機碼
Section Shortcut_Reg
SetOutPath "$INSTDIR"
WriteUninstaller "$INSTDIR\uninstall.exe"

CreateShortcut "$DESKTOP\${APP_NAME}.lnk" "$INSTDIR\SimMAGIC eBook.exe" "" "$INSTDIR\Shortcut.ico"
CreateDirectory "$SMPROGRAMS\${APP_NAME}"
CreateShortcut "$SMPROGRAMS\${APP_NAME}\${APP_NAME}.lnk" "$INSTDIR\SimMAGIC eBook.exe" "" "$INSTDIR\Shortcut.ico"
CreateShortcut "$SMPROGRAMS\${APP_NAME}\Uninstall.lnk" "$INSTDIR\uninstall.exe"
CreateShortcut "$SMPROGRAMS\${APP_NAME}\Language Selector.lnk" "$INSTDIR\Tools.Hamastar.LanguageSelector.exe"

WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayName" "${APP_NAME}"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "UninstallString" "$INSTDIR\uninstall.exe"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayIcon" "$INSTDIR\Shortcut.ico"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "AppPath" "$INSTDIR\SimMAGIC eBook.exe"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayVersion" "${VERSION}"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "Publisher" "${COMP_NAME}"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "URLInfoAbout" "${WEB_SITE}"

# office ptt 解析度
WriteRegDWORD ${REG_ROOT} "Software\Microsoft\Office\8.0\PowerPoint\Options" "ExportBitmapResolution" "200"   
WriteRegDWORD ${REG_ROOT} "Software\Microsoft\Office\9.0\PowerPoint\Options" "ExportBitmapResolution" "200"   
WriteRegDWORD ${REG_ROOT} "Software\Microsoft\Office\10.0\PowerPoint\Options" "ExportBitmapResolution" "200"   
WriteRegDWORD ${REG_ROOT} "Software\Microsoft\Office\11.0\PowerPoint\Options" "ExportBitmapResolution" "200"   
WriteRegDWORD ${REG_ROOT} "Software\Microsoft\Office\12.0\PowerPoint\Options" "ExportBitmapResolution" "200"  
WriteRegDWORD ${REG_ROOT} "Software\Microsoft\Office\14.0\PowerPoint\Options" "ExportBitmapResolution" "200"
WriteRegDWORD ${REG_ROOT} "Software\Microsoft\Office\15.0\PowerPoint\Options" "ExportBitmapResolution" "200"   

# 寫入多語系對應機碼
WriteRegStr ${REG_ROOT} "Software\Hamastar\${APP_ConfigProductVersion}\GlobalResources" "Culture" "$(GlobalLang)"
WriteRegStr ${REG_ROOT} "Software\Hamastar\${APP_ConfigProductVersion}\GlobalResources" "Theme" "blue"

SectionEnd

Function .onInstSuccess
# 靜默完裝完自動執行
IfSilent 0 +2
  Exec "$INSTDIR\SimMAGIC eBook.exe"
FunctionEnd

######################################################################

Section "Uninstall"
${INSTALL_TYPE}

RmDir /r "$INSTDIR"
RmDir /r "$SMPROGRAMS\${APP_NAME}"
Delete "$DESKTOP\${APP_NAME}.lnk"

DeleteRegKey ${REG_ROOT} "Software\Hamastar\${APP_ConfigProductVersion}"
DeleteRegKey ${REG_ROOT} "${UNINSTALL_PATH}"

!insertmacro APP_UNASSOCIATE "sd" "SimMAGIC.sd"
SectionEnd

######################################################################

Function un.onInit
!insertmacro MUI_UNGETLANGUAGE
FunctionEnd

######################################################################


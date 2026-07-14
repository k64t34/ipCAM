function Now {return $(Get-Date -Format  "dd.MM.yyyy HH:mm:ss")}
$Version=20251121

Write-Host $(Now)
Write-Host "***************************************************************************" -ForegroundColor Cyan
Write-Host "Set VLC param                                       by Skorik 2025  $Version"  -ForegroundColor Cyan
Write-Host "***************************************************************************"  -ForegroundColor Cyan
Write-Host $MyInvocation.MyCommand.Definition  -ForegroundColor Green 

Write-Host "This script open file vlcrc and set conifg for VLC: "
Write-Host "1. No update "
Write-Host "2. No window with errors "
Write-Host "3. Stop use GPU while hided"

$Hostname = Read-Host "Enter host name"
#$Hostname="o203-skorik"
Write-Host "You enter [$Hostname]"

if (![string]::IsNullOrEmpty($Hostname)) {
    $h=Test-NetConnection -ComputerName $Hostname | Select-Object -Property RemoteAddress,PingSucceeded
	if ($h.PingSucceeded) {
        $UsersFolder="\\"+$Hostname+"\c$\users"
        if ( [System.IO.Directory]::Exists($UsersFolder)) {
            $Folders =  Get-ChildItem -Path $UsersFolder 
            # | Select-Object  Name #| Format-Table -HideTableHeaders            
            foreach ($f in $Folders){
                Write-Host $f.Name
            }
            $UserName = Read-Host "Enter user name"
            #$UserName="skorik"
            Write-Host "You enter [$UserName]"
            if (![string]::IsNullOrEmpty($UserName)) {
                $UserFolder=$UsersFolder+"\"+$UserName+"\appdata\Roaming\vlc"
                New-Item -Path $UserFolder -ItemType Directory -Force | Out-Null
                if ( [System.IO.Directory]::Exists($UserFolder)) {

                    $FileName=$UserFolder+"\vlcrc"
                    if (! [System.IO.File]::Exists($FileName))   {
                        Write-Host "File"$FileName" not found" -ForegroundColor Yellow  
                        $SourceFile=$PSScriptRoot+"\vlcrc"
                        if ( [System.IO.File]::Exists($SourceFile))   {
                            Write-Host "Copy File"
                            Copy-Item -Path $SourceFile -Destination $FileName
                        }
						else { Write-Host "No source file $SourceFile" -ForegroundColor Red }
					}                    
                    if ( [System.IO.File]::Exists($FileName))   {
                        Write-Host "Open File$FileName"
                        Get-ChildItem -Path $FileName  | `
                        ForEach-Object { `
                            (Get-Content $_) -replace "^\s*qt-error-dialogs\s*=\s*1\s*$", "qt-error-dialogs=0" | `
                            Set-Content $_
                        }
                        Get-ChildItem -Path $FileName  | `
                        ForEach-Object { `
                            (Get-Content $_) -replace "^\s*qt-updates-notif\s*=\s*1\s*$", "qt-updates-notif=0" | `
                            Set-Content $_
                        }
                        Get-ChildItem -Path $FileName  | `
                        ForEach-Object { `
                            (Get-Content $_) -replace "^\s*qt-pause-minimized\s*=\s*1\s*$", "qt-pause-minimized=0" | `
                            Set-Content $_
						}
                    }    
                    else  {Write-Host "File $FileName not created" -ForegroundColor Red } 
                }
                else  {Write-Host "Folder "$UserFolder" not found" -ForegroundColor Red  } 
            }    
            else {Write-Host "Incorrect user name " -ForegroundColor Red  }
        }
        else  {Write-Host "Folder "$UsersFolder" not found" -ForegroundColor Red  }   
    }
    else  {Write-Host "no ping " -ForegroundColor Red  }   
}
else {Write-Host "Incorrect Host name " -ForegroundColor Red  }


Write-Host
Write-Host "finish"
<#
qt-updates-notif=0
qt-error-dialogs=0
qt-pause-minimized=0

Из %APPDATA%\vlc\vlcrc → в C:\ProgramData\vlc\.
Чтобы принудительно применить общие настройки, удалите персональные vlcrc у всех пользователей.


#>
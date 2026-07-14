#!/bin/bash
no__DEBUG=1
FileScript=$(basename $0)
FolderScript="$(dirname "$(realpath $0)")"
_color_red='\e[31'
_color_green='\e[32'
_color_yellow='\e[33'
_color_cyan='\e[36'
_color_white='\e[37'
_color_reset='\e[0m'
_color_bright=';1'
echo -e ${_color_cyan}${_color_bright}m
echo -e "*************************************************************"
echo -e "Script for view IPCam over  VLC     (c)Skorik  ver 2025.06.27"
echo -e "*************************************************************${_color_reset}"
echo -e "${_color_white}mScript ${FolderScript}/${FileScript}${_color_reset}"
[ $__DEBUG ] && echo Debug ON 
#exec 1>>/home/camviwer/cam.log
#exec 2>>/home/camviwer/cam.log
csv="$FolderScript/cam.csv"
FolderDesktop="/home/$USER/Desktop"
ShortcutPrefix='CAM_'
FileAutorun="/home/$USER/.config/autostart/VLC.desktop"
CommandVLC='/snap/bin/vlc --no-audio --fullscreen --loop --repeat --no-interact'
autorun=CamAutoRUN.sh
#for test rtsp://guest:guest@10.80.20.39:554/onvif-media/media.amp?profile=quality_h264&sessiontimeout=0&streamtype=unicast
#********************************
stripquotes() {		
	local str=`echo $*`
	if [[ ${value:0:1} == \" && ${value:$len-1:1} == \" || ${value:0:1} == \' && ${value:$len-1:1} == \' ]] ;then 
	echo ${str:1:-2}
	else
	echo $str
	fi
	
	#local len=$((${#str} - 2))
	#str=${str:1:$len}
	#echo str=$str
	
	# Супер в одно действие	
	#echo ${str:1:-1}

	# Нужна проверка что первый и последний символы действительно кавычки
	#[[ ${value:0:1} == \" && ${value:$len-1:1} == \" ]] && declare -g $1="${value:1:$len-2}"
}
#********************************
function CreateShorcuts () {
[ ! -f $csv ] && { echo -e "${_color_red}mFile $csv not found$_color_reset";read -t30 -n 1 -s -r -p "Press any key to exit..."  ; exit ; }
echo "Delete previous shortcuts from desktop $FolderDesktop"	
find  "$FolderDesktop" -type f  -name '$ShortcutPrefix*' -delete
echo "Create Shortcuts for run VLC from $csv"	

oldIFS=$IFS
IFS=';'
set i_line=0
while read   c_id c_ip c_title c_name c_url line #https://www.opennet.ru/docs/RUS/bash_scripting_guide/c5358.html#READREDIR
  do        
  i_line=$((i_line+1))
  if (($i_line == 1)); then continue;fi
  [ $__DEBUG ] ;echo  "$c_id $c_name $c_title"
  [ ! "$c_id" ] &&  continue
  [ ! "$c_name" ] &&  continue
  [ ! "$c_title" ] &&  continue
  CreateShorcut $(stripquotes $c_name) $(stripquotes $c_title)
  done <$csv 
IFS=$oldIFS  
}
#*********************************
function CreateShorcut () {
shortcut="$FolderDesktop/$ShortcutPrefix$1.desktop"
echo "Create Shortcut $shortcut for cam $2"
echo '[Desktop Entry]'>$shortcut
echo 'Version=1.0'>>$shortcut
echo 'Type=Application'>>$shortcut
echo "Name=$2">>$shortcut
echo 'Comment='>>$shortcut
echo "Exec=/home/sa/Documents/Cam.sh -s $1">>$shortcut
echo 'Icon=vlc'>>$shortcut
echo 'Path=/home/sa'>>$shortcut
echo 'Terminal=true'>>$shortcut
echo 'StartupNotify=false'>>$shortcut
chmod +x $shortcut
gio  set $shortcut metadata::trusted true
}
#**********************************
function RunCam() {
#**********************************
[ $__DEBUG ] && echo RunCam 
[ ! -f $csv ] && { echo -e "${_color_red}mFile $csv not found$_color_reset";read -t30 -n 1 -s -r -p "Press any key to exit..."  ; exit ; }
if [ "$1" == "" ] 
then	
	echo -e "${_color_red}mNo ipcam specified$_color_reset"
	read -t30 -n 1 -s -r -p "Press any key to exit..."   
	exit 2
fi
oldIFS=$IFS
IFS=';'
IsCam=0
while read c_id c_ip c_title c_hostname c_protocol line #https://www.opennet.ru/docs/RUS/bash_scripting_guide/c5358.html#READREDIR
do		
	if [ "$1" = "$c_hostname" ];then		#if [ "$1" == "$(stripquotes $c_hostname)" ];then
		IsCam=1
		break;
	fi
done  <  $csv
IFS=$oldIFS  
if ((IsCam == 0)) then
echo -e "${_color_red}mNo ipcam found with name $1$_color_reset"
exit 1
fi
c_url=$c_protocol			#echo Find property for $1 url=$c_url
c_url=`stripquotes $c_url`;	#echo $c_url
[ $__DEBUG ] && echo Ping #PING				
if ! ping -c 1 $c_ip > /dev/null 2>&1; then	
	echo -e "${_color_red}mNo ping to ${c_ip}$_color_reset"	
	read -n1 -t30 -s -r -p "Press any key to exit..." 
	exit 3
fi		
#Подстановка IP
[ $__DEBUG ] && echo Subst
c_url=${c_url//\[IP\]/$(stripquotes $c_ip)}
[ $__DEBUG ] && read -t30 -n 1 -s -r -p "400"  
echo -e "${_color_white}${_color_bright}mRun VLC for ipcam ${1} ${CommandVLC} ${c_url}${_color_reset}"		
#if [[ ! $__DEBUG ]]; then
	eval "setsid $CommandVLC $c_url >/dev/null 2>&1  & "
	#setsid "$CommandVLC $c_url"		# > nul 2>&1 
#fi
[ $__DEBUG ] && read -t30 -n 1 -s -r -p "500"  
#Create shortcut for autorun
shortcut=$FileAutorun
echo "Create autorun shortcut $shortcut for ipcam $1"
echo '[Desktop Entry]'>$shortcut
echo 'Version=1.0'>>$shortcut
echo 'Type=Application'>>$shortcut
echo "Name=vlc">>$shortcut
echo "Comment=Made by script">>$shortcut
echo "Exec=$CommandVLC $c_url">>$shortcut
echo 'Icon=vlc'>>$shortcut
echo 'Path='>>$shortcut
echo 'Terminal=false'>>$shortcut
echo 'StartupNotify=false'>>$shortcut
[ ! -f $shortcut ] && { echo "File $shortcut not created"
echo -e "${_color_red}mFile $shortcut not created$_color_reset";}
chmod +x $shortcut
}
#*********************************
#exec 1>>/home/camviwer/cam.log
#exec 2>>/home/camviwer/cam.log
#****** S T A R T ***********
echo $(date +"%d.%m.%y %H:%M:%S") Start 
if [ $# -eq 0 ]
then	
	CreateShorcuts
elif [ "$1" == "-s" ] 
	then	
	RunCam $2	
else
	echo 'Cam.sh -s <idIPcam> run VLC for cam <idIPcam> and set it as autostart'
	echo "Cam.sh create link for all IPcams in file $csv"
fi

echo -e "${_color_green}m$(date +"%d.%m.%y %H:%M:%S") Finish $_color_reset"
read -t30 -n 1 -s -r -p "Press any key to continue..."  
echo
exit 

#if [ "$autorun" != "$FileScript" ]
#then
#    echo "Copy file $FileScript to $autorun"
#    cp -f $FileScript $autorun
#fi
#/usr/bin/vlc --no-audio --fullscreen --loop --repeat --no-interact  --started-from-file  rtsp://guest:guest@10.80.20.39:554/onvif-media/media.amp?profile=quality_h264&sessiontimeout=0&streamtype=unicast


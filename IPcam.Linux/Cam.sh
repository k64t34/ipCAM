#!/bin/bash
#TODO Заменить путь '/home/camviwer на /home/<curent user>
echo '*************************************************************'
echo 'Script for IPCam over  VLC'
echo '*************************************************************'
#exec 1>>/home/camviwer/cam.log
#exec 2>>/home/camviwer/cam.log
csv="CamList.csv"
FolderDesktop='/home/camviwer/Рабочий стол'
ShortcutPrefix='CAM_'
FileAutorun='/home/camviwer/.config/autostart/VLC.desktop'
CommandVLC='/usr/bin/vlc --no-audio --fullscreen --loop --repeat --no-interact'
#  --started-from-file'  
#rtsp://guest:guest@10.80.20.39:554/onvif-media/media.amp?profile=quality_h264&sessiontimeout=0&streamtype=unicast
#********************************
stripquotes() {		
	local str=`echo $*`
	#local len=$((${#str} - 2))
	#str=${str:1:$len}
	#echo $str
	
	# Супер в одно действие	
	echo ${str:1:-1}

	# Нужна проверка что первый и последний символы действительно кавычки
	#[[ ${value:0:1} == \" && ${value:$len-1:1} == \" ]] && declare -g $1="${value:1:$len-2}"
}
#********************************
function CreateShorcuts () {
echo "Delete previous shortcuts from desktop $FolderDesktop"	
find  "$FolderDesktop" -type f  -name '$ShortcutPrefix*' -delete
echo "Create Shortcuts for run VLC from $csv"	
[ ! -f $csv ] && { echo "$csv file not found"; exit ; }
oldIFS=$IFS
IFS=';'
cat $csv|while read c_ip c_title c_name c_url line #https://www.opennet.ru/docs/RUS/bash_scripting_guide/c5358.html#READREDIR
  do    
  CreateShorcut $(stripquotes $c_name) $(stripquotes $c_title)
  done  
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
echo "Exec=/home/camviwer/Cam.sh -s $1">>$shortcut
echo 'Icon=vlc'>>$shortcut
echo 'Path=/home/camviwer'>>$shortcut
echo 'Terminal=false'>>$shortcut
echo 'StartupNotify=false'>>$shortcut
chmod +x $shortcut
}
#**********************************
function RunCam() {
#**********************************
if [ "$1" == "" ] 
then
	echo 'No cam specified'
	exit
fi
echo "run VLC for cam $1"
[ ! -f $csv ] && { echo "$csv file not found"; exit ; }
oldIFS=$IFS
IFS=';'
IsCam=0
cat $csv|while read c_ip c_title c_name c_url line #https://www.opennet.ru/docs/RUS/bash_scripting_guide/c5358.html#READREDIR
do 	
	if [ "$1" == "$(stripquotes $c_name)" ];then
		IsCam=1			
		echo Find property for $1 url=$c_url		
		#if [[ $c_url = *?* ]] 
		#then
		#	echo Cam URL=$c_url
		#else
		c_url=`stripquotes $c_url`
		c_url=${c_url:0:-1}
		#fi
		echo Cam URL=$c_url
		#Подстановка IP
		c_url=${c_url//\[IP\]/$(stripquotes $c_ip)}
		echo "$CommandVLC $c_url"
		#eval "$CommandVLC $c_url" & exit
		eval "$CommandVLC $c_url"
				
		shortcut=$FileAutorun
		echo "Create autorun Shortcut $shortcut for cam $2"
		echo '[Desktop Entry]'>$shortcut
		echo 'Version=1.0'>>$shortcut
		echo 'Type=Application'>>$shortcut
		echo "Name=vlc">>$shortcut
		echo "Comment=">>$shortcut
		echo "Exec=$CommandVLC $c_url">>$shortcut
		echo 'Icon=vlc'>>$shortcut
		echo 'Path='>>$shortcut
		echo 'Terminal=false'>>$shortcut
		echo 'StartupNotify=false'>>$shortcut
		chmod +x $shortcut	
		
		break 

	fi  
done  
IFS=$oldIFS  

}
#*********************************
#exec 1>>/home/camviwer/cam.log
#exec 2>>/home/camviwer/cam.log
autorun=CamAutoRUN.sh
echo '*************************************************************'
echo 'Script for IPCam over  VLC'
echo '*************************************************************'
echo ' Cam.sh -s <cam> run VLC for cam <cam>'
echo ' Cam.sh [-1 <cam>] create link and set <cam> as autostart'
echo $(date +"%d.%m.%y %H:%M:%S") Start 
FileScript=$(basename $0)
if [ $# -eq 0 ]
then
	CreateShorcuts
elif [ "$1" == "-s" ] 
	then
	RunCam $2
	
fi

echo $(date +"%d.%m.%y %H:%M:%S") Finish
#read -t30 -s -p "Press any key to continue..." -n 1 dummy
echo
exit 

echo "Script $FileScript"
#echo "autorun  $autorun"
if [ "$1" != "-s" ]
then
	if [ "$2" != "" ]
	then
	echo 'Camera not specified'	
	fi
fi


#if [ "$autorun" != "$FileScript" ]
#then
#    echo "Copy file $FileScript to $autorun"
#    cp -f $FileScript $autorun
#fi
#/usr/bin/vlc --no-audio --fullscreen --loop --repeat --no-interact  --started-from-file  rtsp://guest:guest@10.80.20.39:554/onvif-media/media.amp?profile=quality_h264&sessiontimeout=0&streamtype=unicast


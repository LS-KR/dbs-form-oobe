import os
import time
import requests
import random

def IsFileExist(FileName):
    #To check a file exist
    if(os.path.isfile("C:\\IDS\\SR.txt") == True):
        return True
    else:
        return False

def UploadToServer():
    #To upload data to server
    localtime = time.asctime( time.localtime(time.time()))
    #定义变量
    num = 8
    ran = []
    ran = ''.join(str(i) for i in random.sample(range(0,9),num))
    #读取文件
    with open("systeminfomation.dll", "r") as f :
        IDD = f.read()
    with open("oeminfomation.dll", "r") as f:
        typ = f.read()
    if os.path.exists("C:\IDS") == False:
        randisk = "C:\IDS"
        os.mkdir(randisk)
    #准备上传
    ip = requests.get(url="http://ip.42.pl/raw").text

    typee = typ[typ.rfind('data:'):]

    ty = typee.replace('data:', '')

    IDC = IDD.replace(' ', '')

    ID = IDC.replace('产品ID:', '')

    # 测试版 KEY = ("dbs1145142333")

    KEY = ("dbs-s11451419198102333")

    os.system("del /f /s /q C:\\IDS\\ID.txt")
    os.system("echo "+ran+" >> C:\\IDS\\ID.txt")
    url = "https://log.drblack-system.com/"
    data = {"ID":ID, "type":ty, "time":localtime, "ip":ip, "random":ran, "key-s":KEY}
    r = requests.post(url=url,data=data)

    print(r.text)
    os.system("del /f /s /q \"C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\StartUp\\dbs_form.lnk\"")    
    time.sleep(1.5)
    return 0


if(IsFileExist("C:\\IDS\\SR.txt") == True):
    UploadToServer()
else:
    isavi = 0
    month = 0
    day = 0
    birthfile = open("C:\\IDS\\SR.txt", "w")
    print("The reserved switch is not detected, please enter your birthday as prompted:")
    while(isavi == 0):
        month = int(input("Please enter the month:"))
        day = int(input("Please enter the date:"))
        #检测日期是否合法
        if(month > 0 and month < 13):
            if(month == 1 or month == 3 or month == 5 or month == 7 or month == 8 or month == 10 or month == 12):
                if(day > 0 and day < 32):
                    isavi = 1
            else:
                if(month == 2):
                    if(day > 0 and day < 30):
                        isavi = 1
                else:
                    if(day > 0 and day < 31):
                        isavi = 1
        if(isavi == 0):
            print("Illegal date! Please check the input.")
    if(month < 10):
        upstr = "0" + (str(month) + "-" + str(day))
    else:
        upstr = (str(month) + "-" + str(day))
    birthfile.write(upstr)
    birthfile.close()
    UploadToServer()

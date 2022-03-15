#include <iostream>
#include <objbase.h>
#include <stdio.h>
#include <stdlib.h>
#include <conio.h>
#include <windows.h>
#include <time.h>

#define SW_MODEL SW_SHOWNORMAL
#pragma warning(disable:4996)

#define F(x, y, z) (((x) & (y)) | ((~x) & (z)))
#define G(x, y, z) (((x) & (z)) | ((y) & (~z)))
#define H(x, y, z) ((x) ^ (y) ^ (z))
#define I(x, y, z) ((y) ^ ((x) | (~z)))

#define RL(x, y) (((x) << (y)) | ((x) >> (32 - (y)))) //x向左循环移y位

#define PP(x) (x<<24)|((x<<8)&0xff0000)|((x>>8)&0xff00)|(x>>24) //将x高低位互换,例如PP(aabbccdd)=ddccbbaa

#define FF(a, b, c, d, x, s, ac) a = b + (RL((a + F(b,c,d) + x + ac),s))
#define GG(a, b, c, d, x, s, ac) a = b + (RL((a + G(b,c,d) + x + ac),s))
#define HH(a, b, c, d, x, s, ac) a = b + (RL((a + H(b,c,d) + x + ac),s))
#define II(a, b, c, d, x, s, ac) a = b + (RL((a + I(b,c,d) + x + ac),s))

unsigned A, B, C, D, a, b, c, d, i, len, flen[2], x[16]; //i临时变量,len文件长,flen[2]为64位二进制表示的文件初始长度
char* filename; //文件名
FILE* fp;

void md5() //MD5核心算法,供64轮
{

    a = A, b = B, c = C, d = D;
    /**//* Round 1 */
    FF(a, b, c, d, x[0], 7, 0xd76aa478); /**//* 1 */
    FF(d, a, b, c, x[1], 12, 0xe8c7b756); /**//* 2 */
    FF(c, d, a, b, x[2], 17, 0x242070db); /**//* 3 */
    FF(b, c, d, a, x[3], 22, 0xc1bdceee); /**//* 4 */
    FF(a, b, c, d, x[4], 7, 0xf57c0faf); /**//* 5 */
    FF(d, a, b, c, x[5], 12, 0x4787c62a); /**//* 6 */
    FF(c, d, a, b, x[6], 17, 0xa8304613); /**//* 7 */
    FF(b, c, d, a, x[7], 22, 0xfd469501); /**//* 8 */
    FF(a, b, c, d, x[8], 7, 0x698098d8); /**//* 9 */
    FF(d, a, b, c, x[9], 12, 0x8b44f7af); /**//* 10 */
    FF(c, d, a, b, x[10], 17, 0xffff5bb1); /**//* 11 */
    FF(b, c, d, a, x[11], 22, 0x895cd7be); /**//* 12 */
    FF(a, b, c, d, x[12], 7, 0x6b901122); /**//* 13 */
    FF(d, a, b, c, x[13], 12, 0xfd987193); /**//* 14 */
    FF(c, d, a, b, x[14], 17, 0xa679438e); /**//* 15 */
    FF(b, c, d, a, x[15], 22, 0x49b40821); /**//* 16 */

                                               /**//* Round 2 */
    GG(a, b, c, d, x[1], 5, 0xf61e2562); /**//* 17 */
    GG(d, a, b, c, x[6], 9, 0xc040b340); /**//* 18 */
    GG(c, d, a, b, x[11], 14, 0x265e5a51); /**//* 19 */
    GG(b, c, d, a, x[0], 20, 0xe9b6c7aa); /**//* 20 */
    GG(a, b, c, d, x[5], 5, 0xd62f105d); /**//* 21 */
    GG(d, a, b, c, x[10], 9, 0x02441453); /**//* 22 */
    GG(c, d, a, b, x[15], 14, 0xd8a1e681); /**//* 23 */
    GG(b, c, d, a, x[4], 20, 0xe7d3fbc8); /**//* 24 */
    GG(a, b, c, d, x[9], 5, 0x21e1cde6); /**//* 25 */
    GG(d, a, b, c, x[14], 9, 0xc33707d6); /**//* 26 */
    GG(c, d, a, b, x[3], 14, 0xf4d50d87); /**//* 27 */
    GG(b, c, d, a, x[8], 20, 0x455a14ed); /**//* 28 */
    GG(a, b, c, d, x[13], 5, 0xa9e3e905); /**//* 29 */
    GG(d, a, b, c, x[2], 9, 0xfcefa3f8); /**//* 30 */
    GG(c, d, a, b, x[7], 14, 0x676f02d9); /**//* 31 */
    GG(b, c, d, a, x[12], 20, 0x8d2a4c8a); /**//* 32 */

                                               /**//* Round 3 */
    HH(a, b, c, d, x[5], 4, 0xfffa3942); /**//* 33 */
    HH(d, a, b, c, x[8], 11, 0x8771f681); /**//* 34 */
    HH(c, d, a, b, x[11], 16, 0x6d9d6122); /**//* 35 */
    HH(b, c, d, a, x[14], 23, 0xfde5380c); /**//* 36 */
    HH(a, b, c, d, x[1], 4, 0xa4beea44); /**//* 37 */
    HH(d, a, b, c, x[4], 11, 0x4bdecfa9); /**//* 38 */
    HH(c, d, a, b, x[7], 16, 0xf6bb4b60); /**//* 39 */
    HH(b, c, d, a, x[10], 23, 0xbebfbc70); /**//* 40 */
    HH(a, b, c, d, x[13], 4, 0x289b7ec6); /**//* 41 */
    HH(d, a, b, c, x[0], 11, 0xeaa127fa); /**//* 42 */
    HH(c, d, a, b, x[3], 16, 0xd4ef3085); /**//* 43 */
    HH(b, c, d, a, x[6], 23, 0x04881d05); /**//* 44 */
    HH(a, b, c, d, x[9], 4, 0xd9d4d039); /**//* 45 */
    HH(d, a, b, c, x[12], 11, 0xe6db99e5); /**//* 46 */
    HH(c, d, a, b, x[15], 16, 0x1fa27cf8); /**//* 47 */
    HH(b, c, d, a, x[2], 23, 0xc4ac5665); /**//* 48 */

                                              /**//* Round 4 */
    II(a, b, c, d, x[0], 6, 0xf4292244); /**//* 49 */
    II(d, a, b, c, x[7], 10, 0x432aff97); /**//* 50 */
    II(c, d, a, b, x[14], 15, 0xab9423a7); /**//* 51 */
    II(b, c, d, a, x[5], 21, 0xfc93a039); /**//* 52 */
    II(a, b, c, d, x[12], 6, 0x655b59c3); /**//* 53 */
    II(d, a, b, c, x[3], 10, 0x8f0ccc92); /**//* 54 */
    II(c, d, a, b, x[10], 15, 0xffeff47d); /**//* 55 */
    II(b, c, d, a, x[1], 21, 0x85845dd1); /**//* 56 */
    II(a, b, c, d, x[8], 6, 0x6fa87e4f); /**//* 57 */
    II(d, a, b, c, x[15], 10, 0xfe2ce6e0); /**//* 58 */
    II(c, d, a, b, x[6], 15, 0xa3014314); /**//* 59 */
    II(b, c, d, a, x[13], 21, 0x4e0811a1); /**//* 60 */
    II(a, b, c, d, x[4], 6, 0xf7537e82); /**//* 61 */
    II(d, a, b, c, x[11], 10, 0xbd3af235); /**//* 62 */
    II(c, d, a, b, x[2], 15, 0x2ad7d2bb); /**//* 63 */
    II(b, c, d, a, x[9], 21, 0xeb86d391); /**//* 64 */

    A += a;
    B += b;
    C += c;
    D += d;

}

int main(int argc, char* argv[])
{
    //FILE * fp;
    HKEY key;
    char product[1024];
    char output[4096]={0};
    if((fp=fopen("oeminfomation.dll","w"))==0)//打开文件 
    {
        std::cout<<"File Open Error!";
        getch();
        return 1;
    }
    //读取注册表 
    auto long int ret = RegOpenKeyEx(HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\OEMInformation", 0, KEY_ALL_ACCESS | KEY_WOW64_64KEY, &key);//打开注册表 
    if (ret != ERROR_SUCCESS) {
        std::cout << "open failed: " << ret << std::endl;
        getch();
        return -1;
    }
    DWORD subkeys_count, max_subkey_len, values_count, max_value_name_len, max_value_len;
    ret = RegQueryInfoKey(key, NULL, NULL, NULL, &subkeys_count, &max_subkey_len,
        NULL, &values_count, &max_value_name_len, &max_value_len, NULL, NULL);
    if (ret != ERROR_SUCCESS) {
        std::cout << "query failed" << std::endl;
        getch();
        return -1;
    }
    std::cout << "subkeys count: " << subkeys_count << std::endl;
    char* subkey = new char[max_subkey_len + 1];
    for (int i = 0; i < subkeys_count; i++) {
        DWORD subkey_len = max_subkey_len + 1;
        RegEnumKeyEx(key, i, subkey, &subkey_len, NULL, NULL, NULL, NULL);
        std::cout << "subkey: " << subkey << std::endl;
        sprintf(output,"subkey\tname:%s",subkey);
    }
    delete subkey;
    char* value_name = new char[max_value_name_len + 1];
    //读取注册表并写入文件 
    for (int i = 0; i < values_count; i++) {
        DWORD value_name_len = max_value_name_len + 1;
        DWORD value_type, value_len;
        RegEnumValue(key, i, value_name, &value_name_len, NULL, &value_type, NULL, &value_len);
        BYTE* value = new BYTE[value_len];
        value_name_len = max_value_name_len + 1;
        RegEnumValue(key, i, value_name, &value_name_len, NULL, &value_type, value, &value_len);
        /*if (value_type == REG_DWORD) {
            std::cout << "value, name: " << value_name << " , data: " << *(DWORD*)value << std::endl;
            sprintf(output, "value\tname:%s\tdata:%ld\n", value_name, *(DWORD*)value);
        }*/
        /*else*/ if (value_type == REG_SZ) {
            std::cout << "value, name: " << value_name << " , data: " << (CHAR*)value << std::endl;
            sprintf(output, "value\tname:%s\tdata:%s\n", value_name,(CHAR*)value);
            if(strcmp(value_name,"Model")==0) fputs(output,fp);
        }
        /*else {
            std::cout << "value, name: " << value_name << " , type: " << value_type << std::endl;
            sprintf(output, "value\tname:%s\tdata:%ld\n", value_name, value_type);
        }*/
        //fputs(output,fp);
    }
    fclose(fp);
    //写入系统信息 
    system("del systeminfomation.dll /f /q");
    if((fp=fopen("systeminfomation.dll","w"))==0)//打开文件 
    {
        std::cout<<"File Open Error!";
        getch();
        return 1;
    }
    ret = RegOpenKeyEx(HKEY_LOCAL_MACHINE, "SYSTEM\\CurrentControlSet\\Control\\ComputerName\\ComputerName", 0, KEY_ALL_ACCESS | KEY_WOW64_64KEY, &key);
    if (ret != ERROR_SUCCESS) {
        std::cout << "open failed: " << ret << std::endl;
        getch();
        return -1;
    }
    ret = RegQueryInfoKey(key, NULL, NULL, NULL, &subkeys_count, &max_subkey_len,
        NULL, &values_count, &max_value_name_len, &max_value_len, NULL, NULL);
    if (ret != ERROR_SUCCESS) {
        std::cout << "query failed" << std::endl;
        getch();
        return -1;
    }
    std::cout << "subkeys count: " << subkeys_count << std::endl;
    subkey = new char[max_subkey_len + 1];
    for (int i = 0; i < subkeys_count; i++) {
        DWORD subkey_len = max_subkey_len + 1;
        RegEnumKeyEx(key, i, subkey, &subkey_len, NULL, NULL, NULL, NULL);
        std::cout << "subkey: " << subkey << std::endl;
        sprintf(output,"subkey\tname:%s",subkey);
    }
    delete subkey;
    value_name = new char[max_value_name_len + 1];
    //读取注册表并写入文件 
    for (int i = 0; i < values_count; i++) {
        DWORD value_name_len = max_value_name_len + 1;
        DWORD value_type, value_len;
        RegEnumValue(key, i, value_name, &value_name_len, NULL, &value_type, NULL, &value_len);
        BYTE* value = new BYTE[value_len];
        value_name_len = max_value_name_len + 1;
        RegEnumValue(key, i, value_name, &value_name_len, NULL, &value_type, value, &value_len);
        /*if (value_type == REG_DWORD) {
            std::cout << "value, name: " << value_name << " , data: " << *(DWORD*)value << std::endl;
            sprintf(output, "value\tname:%s\tdata:%ld\n", value_name, *(DWORD*)value);
        }*/
        /*else*/ if (value_type == REG_SZ) {
            std::cout << "value, name: " << value_name << " , data: " << (CHAR*)value << std::endl;
            char * chp=(CHAR*)value;
            while(*chp!='-') chp++;
            chp+=1;
            sprintf(output, "产品 ID:%s", chp);
            if(strcmp(value_name,"ComputerName")==0) fputs(output,fp);
        }
        /*else {
            std::cout << "value, name: " << value_name << " , type: " << value_type << std::endl;
            sprintf(output, "value\tname:%s\tdata:%ld\n", value_name, value_type);
        }*/
        //fputs(output,fp);
    }
    fclose(fp);
    //system("systeminfo>>systeminfomation.dll");
    /*if((fp=fopen("systeminfomation.dll","r"))==0)
    {
        std::cout<<"System File Open Error!";
        getch();
        return 1;
    }
    do
        fgets(product,1024,fp);
    while(strstr(product,"ID:")==NULL);//判断当行是否为产品ID行 
    fclose(fp);
    if((fp=fopen("systeminfomation.dll","w"))==0)
    {
        std::cout<<"System File Write Error!";
        getch();
        return 1;
    }*/
    //MD5加密 
    /*filename="C:\\Windows\\System32\\imageres.dll";
    if (!(fp = fopen(filename, "rb")))
    {
        printf("Can not open this file!\n"); //以二进制打开文件
        //continue;
        return 1;
    }fseek(fp, 0, SEEK_END); //文件指针转到文件末尾
    if ((len = ftell(fp)) == -1)
    {
        printf("Sorry! Can not calculate files which larger than 2 GB!\n"); //ftell函数返回long,最大为2GB,超出返回-1
        fclose(fp);
        //continue;
        return 1;
    }
    rewind(fp); //文件指针复位到文件头
    A = 0x67452301, B = 0xefcdab89, C = 0x98badcfe, D = 0x10325476; //初始化链接变量
    flen[1] = len / 0x20000000; //flen单位是bit
    flen[0] = (len % 0x20000000) * 8;
    memset(x, 0, 64); //初始化x数组为0
    fread(&x, 4, 16, fp); //以4字节为一组,读取16组数据
    for (i = 0; i < len / 64; i++) //循环运算直至文件结束
    {
        md5();
        memset(x, 0, 64);
        fread(&x, 4, 16, fp);
    }
    ((char*)x)[len % 64] = 128; //文件结束补1,补0操作,128二进制即10000000
    if (len % 64 > 55) md5(), memset(x, 0, 64);
    memcpy(x + 14, flen, 8); //文件末尾加入原文件的bit长度
    md5();
    fclose(fp);
    if((fp=fopen("systeminfomation.dll","w"))==0)
    {
        MessageBox(0,"Cannot write file!","Error!",MB_OK|MB_ICONERROR);
    }
    printf("%08X%08X%08X%08X - NEW", PP(A), PP(B), PP(C), PP(D)); //高低位逆反输出
    fprintf(fp,"产品 ID:%08X%08X%08X%08X - NEW", PP(A), PP(B), PP(C), PP(D)); //高低位逆反输出文件 
    //fputs(product,fp);//输出 
    fclose(fp);*/
    //引用下一个程序 
    ShellExecute(0,"open","netchk.exe",0,0,SW_NORMAL);
    return 0;  
}

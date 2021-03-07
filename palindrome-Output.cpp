bool palindrome ( string s  ){
 if ( len==1 ){  
return true;
}
else if (len==2 ){
s[0]=s[1];
}
else if (len>2 ){
s[0]=s[len-1] ;
return palindrome(substr(s,2,len-2));
}

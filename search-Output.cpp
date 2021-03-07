int search ( int n , int arr , int v  ){
 if ( n < 0 )  
return false;
else if (arr[n]==v&&n>=0 )  
return 1;
else if (arr[n]!=v &&n>=0) 
return search(n-1,arr,v);
}
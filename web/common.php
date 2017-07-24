<?php

if (!defined('JSON_PRETTY_PRINT')) {
	define('JSON_PRETTY_PRINT',0);
}

if (!function_exists('http_response_code'))
{
    function http_response_code($newcode = NULL)
    {
        static $code = 200;
        if($newcode !== NULL)
        {
            header('X-PHP-Response-Code: '.$newcode, true, $newcode);
            if(!headers_sent())
                $code = $newcode;
        }       
        return $code;
    }
}

if (!function_exists('getallheaders')) 
{ 
    function getallheaders() 
    { 
           $headers = ''; 
       foreach ($_SERVER as $name => $value) 
       { 
           if (substr($name, 0, 5) == 'HTTP_') 
           { 
               $headers[str_replace(' ', '-', ucwords(strtolower(str_replace('_', ' ', substr($name, 5)))))] = $value; 
           } 
       } 
       return $headers; 
    } 
}

$db_user = "mixer";
$db_name = "traffic_storm";
$db_pass = "ghbdtn42";


if ($_SERVER['REMOTE_ADDR']=="77.34.162.26") {
   exit(77);
}

//$db_user = "u9444246_default";
//$db_name = "u9444246_default";
//$db_pass = "8cUXLG6W";

?>
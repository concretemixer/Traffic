<? 
header('Content-Type: application/json'); 

$user_id = $_GET["user_id"];
$level = $_GET["level"];
$request_params = array(
'type' => $_GET["type"],
'votes' => $_GET["votes"],
'item' => $_GET["item"],
'currency' => 0,
'access_token' => $_GET["access_token"],
'v' => '5.67'
);
$get_params = http_build_query($request_params);
$result = file_get_contents('https://api.vk.com/method/showOrderBox?'. $get_params);
echo $result;
?>
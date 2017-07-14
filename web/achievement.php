<? 
//header('Content-Type: application/json'); 

$user_id = $_GET["user_id"];
$level = $_GET["level"];
$request_params = array(
'user_id' => $_GET["user_id"],
'activity_id' => $_GET["activity_id"],
'value' => $_GET["value"],
'access_token' => $_GET["access_token"],
'v' => '5.67'
);
$get_params = http_build_query($request_params);
$result = file_get_contents('https://api.vk.com/method/secure.addAppEvent?'. $get_params);
echo $result;
?>
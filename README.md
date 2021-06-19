#  Project_USoMK

<p align="center">
<img src="Assets/z_GitHubImages/project_usomk_ss_git.png" width="720">
</p>
Project_USoMK, izometrik kamera açısına sahip Action-Adventure tarzda bir oyun ortaya çıkarmak için atılan ilk adımdır ve teknoloji demosu olarak geliştirilmeye devam etmektedir. Bu projenin başlangıcında temel olarak, yeni oyun mekanikleri oluşturmak, platform ve puzzle tasarlamak, amaçlanmıştır. Sonraki aşamalarda, dövüş/savaş sistemi, düşmanlar, envanter sistemi, diyalog sistemi, yeni hareket mekanikleri geliştirilmesi planlanmaktadır. Bu gelişmeler göz önünde bulundurularak oyun türü ilerleyen zamanlarda değişebilir.

<br/>  

# Map
<table><tr><th> MAP </th><th>LEVEL -2 : Pressure Plates</th></tr>
<tr> <td> <img src="/Assets/z_GitHubImages/Map_SS2_full_git.png" Width="720;"/> </td>
<td> <img src="/Assets/z_GitHubImages/Map_SS2_part1_git.png"  Width="720;"> </td></tr>
<tr><th> LEVEL -1 : Buttons </th> <th> LEVEL 0 : Objects and Prefabs</th></tr>
<tr><td> <img src="/Assets/z_GitHubImages/Map_SS2_part2_git.png" Width="720;"> </td>
<td> <img src="/Assets/z_GitHubImages/Map_SS2_part3_git.png" Width="720;"> </td>
<tr><th> LEVEL 1-2-3-4 : Basic Examples </th> <th> LEVEL 5 : Test Platformer Level </th></tr>
<tr><td> <img src="/Assets/z_GitHubImages/Map_SS2_part4_git.png" Width="720;"> </td>
<td> <img src="/Assets/z_GitHubImages/Map_SS2_part5_git.png" Width="720;"> </td></tr>
</table>

# Controls
<p align="center">
<img src="Assets/z_GitHubImages/controls_git.png" width="720">
</p>

# Project_USoMK Demo Gameplay
[![Project_USoMK Playlist](/Assets/z_GitHubImages/USOMK_youtube.png)](https://www.youtube.com/watch?v=auG90HygEpo&list=PLrzMY5OA4-O7x6NI5SJ2GlP84X5tN_9cX&index=1)

# Branches
Projede iki adet branch bulunmaktadır. Bunlardan "V0_1(OLD)" projede oluşturulan ilk branchtir. Oyun mekaniklerinin temeli burada kodlanmış, platform ve puzzlelar hazırlanmıştır.
V0.1 sürümünde, bir çok nesnenin birbirine el ile bağlanmasından kaynaklı problemler, tekrarlayan mesh kullanımları, texture boyutları gibi sıkıntılar görülmektedir.
"V0_12" branchinde ise projenin V0.1 versiyonunda yaşanan sıkıntılar giderilmiştir. Bu düzeltmelere kısaca bakmak gerekirse:

  * Observer: There was switched to observer pattern rather than link objects manually using Custom Game Events.Controller objects can now trigger target objects with "string targetID".
  * Object Pooling: Object pooling was planned to prevent the problem since Spawners and Duplicators that instantiate and destroy object causes memory problems.
  * Base classes such as Object Base, Controller Base were coded and integrated to objects used.
  * Color change of materials is optimized using material property block.
  * Mesh was arranged to prevent unnecessary copying of some mesh. Mesh is optimized.
  * Adjusted the quality and size of some textures to reduce memory usage of textures.

# Kullanılan assetler
Projede bir takım üçüncü parti paketler bulunmaktadır. Bunlar:
* Projede kullanılan karakter ve animasyonları Mixamo'dan alındı.
* Spell simgeleri ve spell interactive textureları, Synty Studio - Polygon Dungeon assetinden 1-2 texture referans alınarak, demoya uygun şekilde yeniden tasarlanıp eklendi.
[görsel]
* Requirements keylerın modeli olarak, INSTANT ZOOMIES - Horror Starter Pack FREE assetinden anahtar modeli kullanıldı.


# Profiler Results
As a result of the build, the demo used memory between 185-210 MB. In the memory profiler, a large part of this ram usage is due to the settings of the scene camera such as anti-aliasing and bloom under the name of RenderTexture.

<p align="center">
<img src="/Assets/profilerSonuclar/MemoryProfiler.jpg" width="720">
<img src="/Assets/profilerSonuclar/profiler1.jpg" width="720">
<img src="/Assets/profilerSonuclar/profiler2profilers%20sze.jpg" width="720">
</p>
  
  
# Upcoming

<table><tr><th>Combat System</th> <td> <img src="/Assets/berkaynpc/4_Art/MainMenu/MainMenuUI/workInProgress/combat.png" Width="720;"/> </td> </tr>
  <tr><th>Damage System</th> <td> <img src="/Assets/berkaynpc/4_Art/MainMenu/MainMenuUI/workInProgress/damage.png" Width="720;"/> </td> </tr>
  <tr><th>Inverse Kinematics</th> <td> <img src="/Assets/berkaynpc/4_Art/MainMenu/MainMenuUI/workInProgress/ik_2.png" Width="720;"/> </td> </tr>
  <tr><th>Gravity Zones</th> <td> <img src="/Assets/berkaynpc/4_Art/MainMenu/MainMenuUI/workInProgress/gravityZones.png" Width="720;"/> </td> </tr>  
   <tr><th>Gravity Zones</th> <td> <img src="/Assets/berkaynpc/4_Art/MainMenu/MainMenuUI/workInProgress/wallJump.png" Width="720;"/> </td> </tr>  
  <tr><th>New Spell Types</th> <td> <img src="/Assets/berkaynpc/4_Art/MainMenu/MainMenuUI/workInProgress/spellTypes.png" Width="720;"/> </td> </tr>
</table>




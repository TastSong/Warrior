# Warrior二次开发日志

## 开发环境

* 开发平台 `unity5.6.7`
* 编译工具 `VS2013`
* `Android Studio`版本`3.5.3`
* 游戏测试手机`MEIZI 16th`

## 演示图片

![](https://github.com/TastSong/Warrior/blob/master/DomeImage/1.png)

![](https://github.com/TastSong/Warrior/blob/master/DomeImage/0.png)

## 2020.1.31 上传原始项目

此项目是基于[Taikr](https://www.taikr.com/) 《武士2》的项目的二次开发，开发的目的主要是实现游戏的安卓化。

主要有一下目标：

- [x] 加入`AndroidSDK`,发布安卓端游戏
- [x] 实现基本游戏控制，例如行走、发射技能
- [ ] 给游戏人物加血条
- [ ] 添加游戏加载页面
- [ ] 给游戏添加推出按钮

## 2020.2.1 完成游戏基本控制

1. 导入游戏界面UI
2. 导入`NGUI3.6.8`,由于版本问题，需要修复`NGUI.Sprite`与`UnityEngine.Sprite`类型转化问题
3. 添加游戏控制虚拟手柄
4. 添加四个技能发动按钮

 
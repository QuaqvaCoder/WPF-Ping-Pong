   M S B A M L   `   `   `  ���� C  ?PingPong, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:PingPongPingPong  b ^PresentationFramework.Aero2, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35hKclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Aero2Microsoft.Windows.Themes     PingPong.MainWindow   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     (localclr-namespace:PingPong     ^ThemesKclr-namespace:Microsoft.Windows.Themes;assembly=PresentationFramework.Aero2     U QWpfAnimatedGif, Version=2.0.0.0, Culture=neutral, PublicKeyToken=9e7cd3b544a090dc,gif"http://wpfanimatedgif.codeplex.com  -     �Name$  Window�� � Title$ PingPong��$	��675��$	��900��$��./Fonts/#F77 Minecraft)�$��20(� � WindowStyle$ ThreeDBorderWindow=� � 
ResizeMode$ NoResize=� � 	Resources    AppearingMenuButton �� 
TargetType �� Property �� Value �� 
TargetType	 �� Triggers
 P� Property P� Value N� EnterActions �� Source  TriangleButton��%%  &	        &	 ;    �� " ��.���  � ��
 ���� ��  � ��
 �������  � ��
 �������  � �� �� " ��.R��� #��z��._��� $��Center=�$��Center=�	 P� 
 �@J�IsMouseOver$
 True��. ���  � ��
 �������  � ��
 �   ���  � ��
 �   � �� $ ./Sounds/BtnInFocus.wav?��� " ��.���  � ��
 ���� ��  � ��
 ���� ��  � �� �� " ��.R��� #��z��._��� $��Center=�$��Center=�	 P� 
 �@J�IsMouseOver$
 True�� �� $ Sounds/BtnInFocus.wav?� � Triggers .��� -    �Name$ Field������  �� ImageSource$ ./images/field.png��.+��� -    ��Name$ ShadowedPanel��$	��900�� K� ZIndex$ 1�� J� Opacity$	 0.7Q�����  �� Color$ Black���� -    ��Name$ win_SparklesGif��"  WpfAnimatedGif.ImageBehavior   AnimatedSource$ images/sparkles.gif��   	AutoStart$ False��$y�	Collapsed=�$��0,215,574,0q��� -   $ win_SparklesGif2��$ /images/Sparkles.gif��$ False��$y�	Collapsed=�$��570,215,4,0q��� -   $ win_GIF��$ images/win.gif��$ False��$y�	Collapsed=��� -    ��Name$ WinLabel��$�� ��$��75(�$��67,272,52,256q�
������ $y�	Collapsed=�$ 2������  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$ 	#FF0CD3FF��� $ 	#FF31FF11�� � Offset$ 1Q�r� -    r�Name$
 Lose��$
��Left=�$��345,388,0,28q�$y�	Collapsed=�$	��198�� r� Source$ images/Lose.gif?� r� LoadedBehavior$ Manual=�  r� Stretch$
  Fill=�! r� 
SpeedRatio$! 1Q�" r� IsMuted$" False��$ 3���� -	   $ sadGif��$ images/sad.gif��$ False��#   RepeatBehavior$# 3x��$y�	Collapsed=�$��591,60,66,365q�$ J� RenderTransformOrigin$$ 0.5,0.5&�$ 3��z�V� .~��� �� �� % �� Angle$% -24.439Q�T� �� -
   $ 	LoseLabel��$ ��Вы проиграли :(��$��85(�$��24,273,27,256q�$y�	Collapsed=�
������ $ 3������ $ 0.5,1&�$ 0.5,0&�.��� $	 Red��� $ 	#FFB66025��$ 1Q��� -   $ 
PlayGround��$	��900��$ 2��.+��� -   $ P1ControlsImage��$��/Images/WS3.png��$y�Hidden=�$	��178��$��81��
& �� Top$	& 200��' �� Left$	' 133���� -   $ P2ControlsImage��$��/Images/Arrows3.png��$y�Hidden=�$	��178��$��81��$	& 200��$	' 679���� -   $ 
TimerLabel��$�� ��$	��190(�
���   �$y�	Collapsed=�$��Center=�$��Center=�$	' 365��$	& 106��
������ $	��223��$	��170���� -   $ GameInfo��$�� ��$��36(�
���   �$y�	Collapsed=�$	��180��$	��388��$	' 256��$	& 325��$��SemiExpanded&��� -   $ AI_GIF��$ images/AI.gif��$ False��$	��162��$��81��$	' 137��$	& 200��$y�	Collapsed=��� -   $ AI_PlayGround��$	��900���� -   $ 	MainLabel��$��Пинг - Понг��$��
0,60,0,450q�$��72(�
������ ���	#FFFFE800$��Center=�$ 2��$$ 0.5,0.5&�( J� Effect( 3  -System.Windows.Media.Effects.DropShadowEffect  �� -   ) ��Name$) 
PlayButton��$��Играть��$��330,214,0,0q�$��48(�$
��Left=�$	��Top=�$��86��$	��240��$��2��$ 2��(   ���� $ 0.5,1&�$ 0.5,0&�.��� $ 	#FF0CD3FF��� $ 	#FF31FF11��$ 1Q����� " ��.���  � ��
 ���� ��  � �� �� " ��.R��� #��z��._��� $��Center=�$��Center=�	 P� 
 �@J�IsMouseOver$
 True��. ���  � �� �#CEFF1D �� $ Sounds/BtnInFocus.wav?��� -   $) AuthorInfoButton��$��Об авторе��
������ $
��Left=�$	��Top=�$��48(�$	��312��$��85��$��294,330,0,0q�$��2��$ 2����Z� (   ���� $ 0.5,1&�$ 0.5,0&�.��� $ 	#FF0CD3FF��� $ 	#FF7311FF��$ 1Q����� " ��	 P� 
 �@J�IsMouseOver$
 True��. ���  � �� �	#FFA0D5F9 �� $ Sounds/BtnInFocus.wav?��� -   $) 
ExitButton��$��
Выход��$��345,442,0,0q�$	��Top=�$
��Left=�$��48(�$��87��* �� IsCancel$
* True��$	��210��$��2��$ 2��(   ���� $ 0.5,1&�$ 0.5,0&�.��� $	 Red��� $ 	#FFB66025��$ 1Q����� " ��.���  � ��
 ���� ��  � �� �� " ��.R��� #��z��._��� $��Center=�$��Center=�	 P� 
 �@J�IsMouseOver$
 True��. ���  � ��
 � ��� �� $ Sounds/BtnInFocus.wav?��� -   + ��Name$+ pauseMenuBorder��
�������$��0��$��5��$��5q�$ 2��._��� -   $ pauseMenuCanv��$��0��$��5��$��0,0,0,0q����	#FF1C0E29$y�Visible=�.+��� -   $ pauseMenuContent��$��35(�
�������$ ��Играть заново?��$' 18��$& 35��$y�Hidden=��� -   $) SM_UpperButton��$��Продолжить��$' 85��$	& 121��$��30(�$	��231��$��57��$$ 0.212,0.658&�$y�Hidden=�#��[  �� -   $) SM_LowerButton��$��Назад к меню��$' 76��$	& 196��$��30(�$	��249��$��57��$y�Hidden=�#��[  �� -   $+ mainMenuBorder��
�������$��0��$��5��$��5q�$ 2��._��� -   $ mainMenuCanv��$��0��$��5��$��0,0,0,0q����	#FF1C0E29$y�Visible=�.+��� -   $) 	YesButton��$
��Да��$' 51��$	& 319��$	��200��$��55��$��36(�$y�Hidden=�#��[  �� -   $) NoButton��$��Нет��$	' 339��$	& 319��$	��200��$��55��$��36(�$y�Hidden=�#��[  �� -   $ mainMenuContent��$	��600��$	��200��$	��600��
�������$y�Hidden=�
�������$��35(��� -    $) PlayVsAiButton��$��Против ИИ��$' 51��$	& 138��#��[  $��91��$	��225��$y�Hidden=�$��30(��� -!   $) PlayVs2PButton��$��Против 2И��$	' 326��$	& 138��#��[  $��91��$	��226��$y�Hidden=�$��30(��� -"   $) RightSideButton��$	' 399��$	& 130��$	��115��$	��100��$y�	Collapsed=����� " ��, �� BasedOn#, [ .���  � �� �� $" /images/TriagButtonRight.png��	 P� 
 �@J�IsMouseOver$
 True��. ���  � �� �� $&  /images/TriagButtonMouseOver.png���� -#   $) LeftSideButton��$	' 101��$	& 130��$	��115��$	��100��$y�	Collapsed=����� " ��#, [ .���  � �� �� $! /images/TriagButtonLeft.png��	 P� 
 �@J�IsMouseOver$
 True��. ���  � �� �� $' !/images/TriagButtonMouseOver2.png���� -$   $+ scoreMenuBorder��
���   �$��0��$��5��$��5q�$ 2��._��� -%   $ scoreMenuCanv��$��0��$��5��$��0,0,0,0q����	#FFF9EA52.+��� -&   $ LeftSideName��$' 88��$& 16��$$ 0.006,0.496&�$��80(�$y�Hidden=��� -'   $ RightSideName��$	' 385��$& 16��$��80(�$y�Hidden=��� $	��400��
���   �$��5��$	' 300��$��5��$y�Hidden=��� -(   $ LeftSideScore��$��0��$	& 132��$	��188��$	��285��$' 10��$	��180(�$��Center=�$��Center=�$y�Hidden=��� -)   $ RightSideScore��$��0��$	& 132��$	��188��$	��280��$	' 310��$	��180(�$��Center=�$��Center=�$y�Hidden=�
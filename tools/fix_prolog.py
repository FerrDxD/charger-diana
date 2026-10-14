import pathlib

out = pathlib.Path(r"C:\Users\Hype GLK\charger-diana\scenes\prolog\PrologRoom.tscn")

t = r'''[gd_scene load_steps=32 format=3 uid="uid://c_prolog_room"]

[ext_resource type="Script" path="res://scripts/prolog/PrologRoom.cs" id="1"]
[ext_resource type="PackedScene" path="res://scenes/player/Player.tscn" id="2_player"]
[ext_resource type="PackedScene" path="res://scenes/prolog/PrologChatUi.tscn" id="3_chat"]
[ext_resource type="Script" path="res://scripts/prolog/PrologInteractable.cs" id="4_interact"]
[ext_resource type="PackedScene" uid="uid://cdj8x7nl5qkvw" path="res://scenes/ui/Hud.tscn" id="5_hud"]
[ext_resource type="Theme" uid="uid://bu1c2d3e4f5g6" path="res://resources/ui_theme.tres" id="6_theme"]
[ext_resource type="PackedScene" path="res://scenes/props/prolog/prolog_floor_2m.tscn" id="7_floor"]
[ext_resource type="PackedScene" path="res://scenes/props/prolog/prolog_ceiling_2m.tscn" id="8_ceil"]
[ext_resource type="PackedScene" path="res://scenes/props/prolog/prolog_wall_2m.tscn" id="9_wall"]
[ext_resource type="PackedScene" path="res://scenes/props/prolog/prolog_wall_door_2m.tscn" id="10_wall_door"]
[ext_resource type="PackedScene" path="res://scenes/props/prolog/prolog_wall_window_2m.tscn" id="11_wall_win"]
[ext_resource type="PackedScene" path="res://scenes/props/prolog/prolog_rug.tscn" id="12_rug"]
[ext_resource type="PackedScene" path="res://scenes/props/prolog/prolog_bed_frame.tscn" id="13_bedf"]
[ext_resource type="PackedScene" path="res://scenes/props/prolog/prolog_mattress.tscn" id="14_matt"]
[ext_resource type="PackedScene" path="res://scenes/props/prolog/prolog_duvet.tscn" id="15_duvet"]
[ext_resource type="PackedScene" path="res://scenes/props/prolog/prolog_pillow.tscn" id="16_pillow"]
[ext_resource type="PackedScene" path="res://scenes/props/prolog/prolog_nightstand.tscn" id="17_night"]
[ext_resource type="PackedScene" path="res://scenes/props/prolog/prolog_desk_top.tscn" id="18_deskt"]
[ext_resource type="PackedScene" path="res://scenes/props/prolog/prolog_desk_leg.tscn" id="19_leg"]
[ext_resource type="PackedScene" path="res://scenes/props/prolog/prolog_wardrobe.tscn" id="20_ward"]
[ext_resource type="PackedScene" path="res://scenes/props/prolog/prolog_shelf_110.tscn" id="21_shelf"]
[ext_resource type="PackedScene" path="res://scenes/props/prolog/prolog_chair_seat.tscn" id="22_chair"]
[ext_resource type="PackedScene" path="res://scenes/props/prolog/prolog_door_leaf.tscn" id="23_door"]
[ext_resource type="PackedScene" path="res://scenes/props/prolog/prolog_window_glass.tscn" id="24_glass"]
[ext_resource type="PackedScene" path="res://scenes/props/prolog/prolog_bag.tscn" id="25_bag"]
[ext_resource type="PackedScene" path="res://scenes/props/prolog/prolog_book_stack.tscn" id="26_books"]
[ext_resource type="PackedScene" path="res://scenes/props/prolog/prolog_charger.tscn" id="27_charger"]
[ext_resource type="PackedScene" path="res://scenes/props/prolog/prolog_phone.tscn" id="28_phone"]

[sub_resource type="Environment" id="Environment_1"]
background_mode = 1
background_color = Color(0.015, 0.015, 0.025, 1)
ambient_light_source = 2
ambient_light_color = Color(0.08, 0.08, 0.12, 1)

[sub_resource type="StandardMaterial3D" id="Mat_AccentWall"]
albedo_color = Color(0.82, 0.78, 0.72, 1)
roughness = 0.9
texture_filter = 0

[sub_resource type="BoxShape3D" id="BoxShape3D_bag"]
size = Vector3(0.5, 0.5, 0.5)

[sub_resource type="BoxShape3D" id="BoxShape3D_drawer"]
size = Vector3(0.5, 0.3, 0.35)

[sub_resource type="BoxShape3D" id="BoxShape3D_jacket"]
size = Vector3(0.5, 1.0, 0.2)

[sub_resource type="BoxShape3D" id="BoxShape3D_chair_back"]
size = Vector3(0.04, 0.32, 0.44)

[node name="PrologRoom" type="Node3D"]
script = ExtResource("1")

[node name="WorldEnvironment" type="WorldEnvironment" parent="."]
environment = SubResource("Environment_1")

[node name="RoomLight" type="OmniLight3D" parent="."]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 2.3, 0)
light_color = Color(0.95, 0.9, 0.8, 1)
light_energy = 1.0
omni_range = 6.0
shadow_enabled = false

[node name="PhoneGlow" type="OmniLight3D" parent="."]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 1.1, 0.9, -1.1)
light_color = Color(0.4, 0.65, 0.9, 1)
light_energy = 0.6
omni_range = 2.0
shadow_enabled = false

[node name="DeskLampLight" type="OmniLight3D" parent="."]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0.5, 1.15, -1.7)
light_color = Color(0.95, 0.88, 0.75, 1)
light_energy = 0.5
omni_range = 2.0
shadow_enabled = false

[node name="BedsideLampLight" type="OmniLight3D" parent="."]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, -0.25, 0.55, -1.7)
light_color = Color(1, 0.85, 0.65, 1)
light_energy = 0.4
omni_range = 2.0
shadow_enabled = false

[node name="Shell" type="Node3D" parent="."]

[node name="Floor_A" parent="Shell" instance=ExtResource("7_floor")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, -2, 0, -2)

[node name="Floor_B" parent="Shell" instance=ExtResource("7_floor")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, -2)

[node name="Floor_C" parent="Shell" instance=ExtResource("7_floor")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, -2, 0, 0)

[node name="Floor_D" parent="Shell" instance=ExtResource("7_floor")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0)

[node name="Ceiling_A" parent="Shell" instance=ExtResource("8_ceil")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, -2, 2.8, -2)

[node name="Ceiling_B" parent="Shell" instance=ExtResource("8_ceil")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 2.8, -2)

[node name="Ceiling_C" parent="Shell" instance=ExtResource("8_ceil")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, -2, 2.8, 0)

[node name="Ceiling_D" parent="Shell" instance=ExtResource("8_ceil")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 2.8, 0)

[node name="WallNorth_Window" parent="Shell" instance=ExtResource("11_wall_win")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, -2, 0, -2.12)

[node name="WallEast" parent="Shell" instance=ExtResource("9_wall")]
transform = Transform3D(0, 0, -1, 0, 1, 0, 1, 0, 0, 2, 0, -2)

[node name="WallEast2" parent="Shell" instance=ExtResource("9_wall")]
transform = Transform3D(0, 0, -1, 0, 1, 0, 1, 0, 0, 2, 0, 0)

[node name="WallSouth_Door" parent="Shell" instance=ExtResource("10_wall_door")]
transform = Transform3D(-1, 0, 0, 0, 1, 0, 0, 0, -1, 0.2, 0, 2)

[node name="WallWest" parent="Shell" instance=ExtResource("9_wall")]
transform = Transform3D(0, 0, 1, 0, 1, 0, -1, 0, 0, -2.12, 0, 0)

[node name="WallWest2" parent="Shell" instance=ExtResource("9_wall")]
transform = Transform3D(0, 0, 1, 0, 1, 0, -1, 0, 0, -2.12, 0, -2)

[node name="WindowGlass" parent="Shell" instance=ExtResource("24_glass")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, -1.46, 1.05, -2.11)

[node name="DoorLeaf" parent="Shell" instance=ExtResource("23_door")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, -0.65, 0, 1.94)

[node name="Rug" parent="Shell" instance=ExtResource("12_rug")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, -1, 0.08, -0.85)

[node name="Furniture" type="Node3D" parent="."]

[node name="BedFrame" parent="Furniture" instance=ExtResource("13_bedf")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, -1.85, 0.08, -1.85)

[node name="Mattress" parent="Furniture" instance=ExtResource("14_matt")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, -1.79, 0.33, -1.78)

[node name="Duvet" parent="Furniture" instance=ExtResource("15_duvet")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, -1.79, 0.53, -1.45)

[node name="PillowA" parent="Furniture" instance=ExtResource("16_pillow")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, -1.79, 0.53, -1.85)

[node name="PillowB" parent="Furniture" instance=ExtResource("16_pillow")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, -1.35, 0.53, -1.55)

[node name="Nightstand" parent="Furniture" instance=ExtResource("17_night")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, -0.25, 0.08, -1.85)

[node name="DeskTop" parent="Furniture" instance=ExtResource("18_deskt")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0.75, 0.68, -1.85)

[node name="DeskLeg1" parent="Furniture" instance=ExtResource("19_leg")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0.76, 0.08, -1.55)

[node name="DeskLeg2" parent="Furniture" instance=ExtResource("19_leg")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 1.96, 0.08, -1.55)

[node name="DeskLeg3" parent="Furniture" instance=ExtResource("19_leg")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0.76, 0.08, -1.26)

[node name="DeskLeg4" parent="Furniture" instance=ExtResource("19_leg")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 1.96, 0.08, -1.26)

[node name="Wardrobe" parent="Furniture" instance=ExtResource("20_ward")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 1.45, 0.08, 0.9)

[node name="ShelfLower" parent="Furniture" instance=ExtResource("21_shelf")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0.25, 1.35, -2)

[node name="ShelfUpper" parent="Furniture" instance=ExtResource("21_shelf")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0.25, 1.75, -2)

[node name="BookStackOnDesk" parent="Furniture" instance=ExtResource("26_books")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 1.45, 0.74, -1.85)

[node name="PhoneOnDesk" parent="Furniture" instance=ExtResource("28_phone")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0.9, 0.74, -1.65)

[node name="ChargerOnDesk" parent="Furniture" instance=ExtResource("27_charger")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 1.1, 0.74, -1.6)

[node name="ChairSeat" parent="Furniture" instance=ExtResource("22_chair")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0.75, 0.08, -0.85)

[node name="ChairBack" type="CSGBox3D" parent="Furniture"]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0.98, 0.34, -0.63)
size = Vector3(0.04, 0.32, 0.44)
material = SubResource("Mat_AccentWall")

[node name="Player" parent="." instance=ExtResource("2_player")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0.1, 0)

[node name="Interactables" type="Node3D" parent="."]

[node name="Bag" type="StaticBody3D" parent="Interactables"]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0.25, 0.08, 0.25)
script = ExtResource("4_interact")
ObjectId = "bag"
PromptText = "Periksa Tas"

[node name="BagVisual" parent="Interactables/Bag" instance=ExtResource("25_bag")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, -0.22, 0.19, -0.11)

[node name="Collision" type="CollisionShape3D" parent="Interactables/Bag"]
shape = SubResource("BoxShape3D_bag")

[node name="Glow" type="OmniLight3D" parent="Interactables/Bag"]
light_color = Color(0.45, 0.65, 0.85, 1)
light_energy = 0.5
omni_range = 1.2
shadow_enabled = false

[node name="Drawer" type="StaticBody3D" parent="Interactables"]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0.95, 0.48, -1.65)
script = ExtResource("4_interact")
ObjectId = "drawer"
PromptText = "Periksa Laci"

[node name="Collision" type="CollisionShape3D" parent="Interactables/Drawer"]
shape = SubResource("BoxShape3D_drawer")

[node name="Glow" type="OmniLight3D" parent="Interactables/Drawer"]
light_color = Color(0.85, 0.75, 0.45, 1)
light_energy = 0.5
omni_range = 1.2
shadow_enabled = false

[node name="Jacket" type="StaticBody3D" parent="Interactables"]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, -0.4, 1.3, 1.85)
script = ExtResource("4_interact")
ObjectId = "jacket"
PromptText = "Ambil Jaket & Pergi"

[node name="Collision" type="CollisionShape3D" parent="Interactables/Jacket"]
shape = SubResource("BoxShape3D_jacket")

[node name="JacketMesh" type="CSGBox3D" parent="Interactables/Jacket"]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0, -0.1, 0)
size = Vector3(0.4, 0.9, 0.12)
material = SubResource("Mat_AccentWall")

[node name="Glow" type="OmniLight3D" parent="Interactables/Jacket"]
light_color = Color(0.95, 0.85, 0.6, 1)
light_energy = 0.8
omni_range = 1.8
visible = false
shadow_enabled = false

[node name="UI" type="CanvasLayer" parent="."]
layer = 1

[node name="Hud" parent="UI" instance=ExtResource("5_hud")]

[node name="ChatNotification" type="PanelContainer" parent="UI"]
visible = false
anchors_preset = 5
anchor_left = 0.5
anchor_right = 0.5
offset_left = -220.0
offset_top = 40.0
offset_right = 220.0
offset_bottom = 80.0
grow_horizontal = 2
theme = ExtResource("6_theme")

[node name="Margin" type="MarginContainer" parent="UI/ChatNotification"]
layout_mode = 2
theme_override_constants/margin_left = 12
theme_override_constants/margin_top = 6
theme_override_constants/margin_right = 12
theme_override_constants/margin_bottom = 6

[node name="Label" type="Label" parent="UI/ChatNotification/Margin"]
layout_mode = 2
theme_override_colors/font_color = Color(0.95, 0.85, 0.3, 1)
theme_override_font_sizes/font_size = 18
text = "[!] CHAT DARI SALSA — TEKAN [TAB / M] BUKA HP"
horizontal_alignment = 1

[node name="PrologChatUi" parent="." instance=ExtResource("3_chat")]
'''
out.write_text(t, encoding="utf-8")
print("wrote", out)

import re

with open('scenes/levels/Chapter1.tscn', 'r') as f:
    content = f.read()

# Add ext_resource if not present
resources_to_add = [
    '[ext_resource type="PackedScene" path="res://scenes/interactables/Pickup.tscn" id="5_pickup"]',
    '[ext_resource type="PackedScene" path="res://scenes/interactables/KeyDoor.tscn" id="6_keydoor"]',
    '[ext_resource type="PackedScene" uid="uid://c7t8r9s0u1v2w" path="res://scenes/interactables/PowerPanel.tscn" id="7_power"]',
    '[ext_resource type="PackedScene" path="res://scenes/interactables/NoteReadable.tscn" id="8_note"]'
]

# Find the last ext_resource
last_ext_idx = content.rfind('[ext_resource')
end_of_last_ext = content.find(']', last_ext_idx) + 1

# Insert new resources
insert_str = '\n' + '\n'.join(resources_to_add)
content = content[:end_of_last_ext] + insert_str + content[end_of_last_ext:]

# Replace Interactables node
interactables_replacement = """[node name="Interactables" type="Node3D" parent="."]

[node name="Pickup_KunciLobi" parent="Interactables" instance=ExtResource("5_pickup")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 8, 0, -8)
ItemId = &"kunci_lobi"

[node name="KeyDoor_Lobi" parent="Interactables" instance=ExtResource("6_keydoor")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, -17)
RequiredKey = &"kunci_lobi"

[node name="PowerPanel" parent="Interactables" instance=ExtResource("7_power")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, -25, 1.5, -23)

[node name="Pickup_KunciKelas" parent="Interactables" instance=ExtResource("5_pickup")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 5.5, -18)
ItemId = &"kunci_kelas"

[node name="KeyDoor_Kelas" parent="Interactables" instance=ExtResource("6_keydoor")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, -12, 5, -23)
RequiredKey = &"kunci_kelas"

[node name="ChargerPickup" parent="Interactables" instance=ExtResource("4_charger")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, -12, 4.5, -17)

[node name="Pickup_KunciGerbang" parent="Interactables" instance=ExtResource("5_pickup")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 10, 5.5, -22)
ItemId = &"kunci_gerbang"

[node name="ExitGate" parent="Interactables" instance=ExtResource("3_exit")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, 10, 4.5, -18)
RequiredKey = &"kunci_gerbang"

[node name="Note_Hint" parent="Interactables" instance=ExtResource("8_note")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, -8, 1.5, -25)
NoteId = &"hint_osis"
NoteTitle = "Catatan OSIS"
NoteText = "Kunci kelas ada di ruang OSIS. Jangan lupa nyalakan listrik."

[node name="Pickup_Baterai" parent="Interactables" instance=ExtResource("5_pickup")]
transform = Transform3D(1, 0, 0, 0, 1, 0, 0, 0, 1, -15, 5.5, -25)
ItemId = &"baterai_cadangan\""""

# regex replace
content = re.sub(r'\[node name="Interactables" type="Node3D" parent="\."\].*?\[node name="Enemies" type="Node3D" parent="\."\]', interactables_replacement + '\n\n[node name="Enemies" type="Node3D" parent="."]', content, flags=re.DOTALL)

with open('scenes/levels/Chapter1.tscn', 'w') as f:
    f.write(content)

print("Modified Chapter1.tscn")

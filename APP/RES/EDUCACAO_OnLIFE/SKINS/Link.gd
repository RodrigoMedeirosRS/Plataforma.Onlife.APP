extends TextureButton

export var url = ""

func _ready() -> void:
	pass

func _on_Link_pressed() -> void:
	OS.shell_open(url)
	pass # Replace with function body.

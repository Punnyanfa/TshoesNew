<template>
  <div class="sidebar-custom">
    <!-- Tabs -->
    <div class="tabs">
      <button class="tab-btn" :class="{ active: activeTab === 'text' }" @click="setActiveTab('text')">
        <i class="icon-text">T</i> Text
      </button>
      <button class="tab-btn" :class="{ active: showBackground }" @click="toggleBackground">
        <i class="icon-background">□</i> Background
      </button>
      <button class="tab-btn" :class="{ active: activeTab === 'upload' }" @click="setActiveTab('upload')">
        <i class="icon-upload">☁</i> My Files
      </button>
    </div>

    <!-- Hiển thị background-section khi click vào tab Background -->
    <div v-if="showBackground" class="background-section">
      <div class="color-picker">
        <label for="color-picker">Select Color:</label>
        <input
          id="color-picker"
          type="color"
          v-model="customColor"
          @input="applyBackgroundColor(customColor)"
        />
      </div>
      <p class="selected-color">Selected: {{ selectedBackgroundColor }}</p>
    </div>

    <!-- Tab Content (chỉ giữ Text và Upload) -->
    <div class="tab-content">
      <!-- Upload Image Tab -->
      <div v-if="activeTab === 'upload'" class="upload-section">
        <label for="image-upload" class="upload-btn">Upload Image</label>
        <input
          id="image-upload"
          type="file"
          accept="image/*"
          @change="handleImageUpload"
          style="display: none;"
        />
        <div v-if="uploadedImage" class="uploaded-file-info">
          <div class="image-preview" 
               draggable="true"
               @dragstart="handleDragStart"
               @dragend="handleDragEnd">
            <img :src="imagePreviewUrl" alt="Uploaded Image Preview" />
          </div>
        </div>
        <p v-if="uploadError" class="error-text">{{ uploadError }}</p>
      </div>

      <!-- Text Tab -->
      <div v-if="activeTab === 'text'" class="text-section">
        <input
          v-model="customText"
          type="text"
          placeholder="Enter your text here"
          class="text-input"
          @input="previewText"
        />
        <div class="text-options">
          <label for="text-size">Text Size:</label>
          <input
            id="text-size"
            type="range"
            v-model="textSize"
            min="20"
            max="100"
            step="1"
            @input="updateTextSize"
          />
          <span>{{ textSize }}px</span>
        </div>
        <div class="text-options">
          <label for="text-color">Text Color:</label>
          <input
            id="text-color"
            type="color"
            v-model="textColor"
          />
        </div>
        <button class="apply-btn" @click="createDraggableText" :disabled="!customText || isApplyingText">
          {{ isApplyingText ? 'Applying...' : 'Apply Text' }}
        </button>

        <!-- Hiển thị danh sách các văn bản có thể kéo thả -->
        <div v-if="draggableTexts.length" class="draggable-texts">
          <div
            v-for="(textObj, index) in draggableTexts"
            :key="index"
            class="draggable-text"
            draggable="true"
            @dragstart="startTextDrag($event, textObj, index)"
            @dragend="endTextDrag"
          >
            {{ textObj.text }}
          </div>
        </div>
      </div>

      <!-- Reset Button -->
      <button class="reset-btn" @click="resetModel">Reset</button>
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from 'vue';
import * as THREE from 'three';
import { DecalGeometry } from 'three/examples/jsm/geometries/DecalGeometry.js'; // Import DecalGeometry

const props = defineProps({
  scene: { type: Object, required: true },
  model: { type: Object, default: null },
  originalMaterials: { type: Map, default: () => new Map() },
  camera: { type: Object, required: true },
  renderer: { type: Object, required: true },
});

const emit = defineEmits(['textureApplied', 'textApplied', 'backgroundColorApplied']);

const activeTab = ref('upload');
const uploadedImage = ref(null);
const imagePreviewUrl = ref('');
const uploadError = ref('');
const customText = ref('');
const isApplyingText = ref(false);
const selectedBackgroundColor = ref('#ffffff');
const customColor = ref('#ffffff');
const showBackground = ref(false);
const draggableTexts = ref([]); // Danh sách các văn bản có thể kéo thả (bao gồm text, size, color)
const textSize = ref(60); // Kích thước văn bản mặc định
const textColor = ref('#ffffff'); // Màu văn bản mặc định
const appliedDecals = ref([]); // Lưu trữ các decal để reset

// Hàm toggle hiển thị background-section
const toggleBackground = () => {
  showBackground.value = !showBackground.value;
  if (showBackground.value) {
    activeTab.value = ''; // Bỏ chọn tab khác khi hiển thị background
  }
};

// Hàm set active tab và ẩn background-section
const setActiveTab = (tab) => {
  activeTab.value = tab;
  showBackground.value = false; // Ẩn background-section khi click tab khác
};

// Cập nhật kích thước văn bản
const updateTextSize = () => {
  // Có thể thêm logic preview nếu cần
};

// Tạo văn bản có thể kéo thả
const createDraggableText = () => {
  if (!customText.value) return;
  draggableTexts.value.push({
    text: customText.value,
    size: textSize.value,
    color: textColor.value,
  });
  customText.value = ''; // Xóa input sau khi thêm
};

// Bắt đầu kéo văn bản
const startTextDrag = (event, textObj, index) => {
  event.dataTransfer.setData('text/plain', JSON.stringify(textObj));
  event.dataTransfer.setData('text-index', index); // Lưu index để xóa sau khi thả
};

// Kết thúc kéo văn bản
const endTextDrag = () => {
  // Có thể thêm logic nếu cần
};

// Tạo texture từ văn bản
const createTextTexture = (text, size, color) => {
  const canvas = document.createElement('canvas');
  const context = canvas.getContext('2d');
  canvas.width = size * 10; // Điều chỉnh kích thước canvas dựa trên size
  canvas.height = size * 5;

  context.clearRect(0, 0, canvas.width, canvas.height);
  context.font = `Bold ${size}px Arial`;
  context.fillStyle = color;
  context.textAlign = 'center';
  context.textBaseline = 'middle';
  context.fillText(text, canvas.width / 2, canvas.height / 2);

  const texture = new THREE.CanvasTexture(canvas);
  texture.needsUpdate = true;
  return texture;
};

// Xử lý sự kiện thả văn bản lên canvas
const handleTextDrop = (event) => {
  event.preventDefault();
  const textObj = JSON.parse(event.dataTransfer.getData('text/plain'));
  const textIndex = parseInt(event.dataTransfer.getData('text-index'), 10);

  if (!props.model || !props.camera || !props.renderer) {
    alert('Missing model, camera, or renderer. Please ensure all components are loaded.');
    return;
  }

  // Lấy vị trí chuột trên canvas
  const canvas = props.renderer.domElement;
  const rect = canvas.getBoundingClientRect();
  const mouse = new THREE.Vector2();
  mouse.x = ((event.clientX - rect.left) / rect.width) * 2 - 1;
  mouse.y = -((event.clientY - rect.top) / rect.height) * 2 + 1;

  // Sử dụng raycaster để tìm giao điểm với mô hình
  const raycaster = new THREE.Raycaster();
  raycaster.setFromCamera(mouse, props.camera);
  const intersects = raycaster.intersectObject(props.model, true);

  if (intersects.length > 0) {
    const intersect = intersects[0];
    const mesh = intersect.object; // Mesh mà người dùng thả văn bản lên
    const position = intersect.point; // Vị trí giao điểm
    const normal = intersect.face.normal.clone().applyMatrix4(mesh.matrixWorld).normalize(); // Pháp tuyến tại điểm giao

    // Tạo texture từ văn bản
    const textTexture = createTextTexture(textObj.text, textObj.size, textObj.color);

    // Tạo decal
    const decalMaterial = new THREE.MeshPhongMaterial({
      map: textTexture,
      transparent: true,
      depthTest: true,
      depthWrite: false,
      polygonOffset: true,
      polygonOffsetFactor: -4,
    });

    const decalSize = new THREE.Vector3(textObj.size / 100, textObj.size / 100, 0.1); // Kích thước decal
    const decalGeometry = new DecalGeometry(mesh, position, normal, decalSize);
    const decal = new THREE.Mesh(decalGeometry, decalMaterial);

    props.scene.add(decal);
    appliedDecals.value.push(decal); // Lưu decal để reset

    // Xóa văn bản khỏi danh sách draggable sau khi thả
    draggableTexts.value.splice(textIndex, 1);

    emit('textApplied', textObj.text);
    alert(`Text "${textObj.text}" applied at the selected position!`);
  } else {
    alert('Please drop the text on the model.');
  }
};

// Handle image upload
const handleImageUpload = (event) => {
  const file = event.target.files[0];
  uploadError.value = '';
  if (file) {
    if (!file.type.startsWith('image/')) {
      uploadError.value = 'Vui lòng tải lên một file ảnh hợp lệ.';
      return;
    }
    if (file.size > 5 * 1024 * 1024) {
      uploadError.value = 'Kích thước file vượt quá giới hạn 5MB.';
      return;
    }
    uploadedImage.value = file;

    const reader = new FileReader();
    reader.onload = (e) => {
      imagePreviewUrl.value = e.target.result;
      emit('textureApplied', e.target.result);
    };
    reader.readAsDataURL(file);
  }
};

// Apply background color to the model
const applyBackgroundColor = (color) => {
  if (!props.model) {
    alert('No model loaded. Please load a model first.');
    return;
  }

  props.model.traverse((child) => {
    if (child.isMesh) {
      const newMaterial = new THREE.MeshStandardMaterial({
        color: new THREE.Color(color),
        metalness: 0.2,
        roughness: 0.8,
      });
      child.material.dispose();
      child.material = newMaterial;
      child.material.needsUpdate = true;
    }
  });

  selectedBackgroundColor.value = color;
  emit('backgroundColorApplied', color);
  alert(`Color "${color}" applied in the model!`);
};

// Reset model to original state
const resetModel = () => {
  if (!props.model) {
    alert('No model loaded. Nothing to reset.');
    return;
  }

  props.model.traverse((child) => {
    if (child.isMesh && props.originalMaterials.has(child)) {
      child.material.dispose();
      child.material = props.originalMaterials.get(child).clone();
      child.material.needsUpdate = true;
    }
  });

  // Xóa tất cả các decal
  appliedDecals.value.forEach((decal) => {
    props.scene.remove(decal);
    decal.material.dispose();
    decal.geometry.dispose();
  });
  appliedDecals.value = [];

  uploadedImage.value = null;
  imagePreviewUrl.value = '';
  customText.value = '';
  draggableTexts.value = [];
  selectedBackgroundColor.value = '#ffffff';
  customColor.value = '#ffffff';
  showBackground.value = false;
  textSize.value = 60; // Reset kích thước văn bản
  textColor.value = '#ffffff'; // Reset màu văn bản

  const imageUploadInput = document.getElementById('image-upload');
  if (imageUploadInput) {
    imageUploadInput.value = '';
  }

  alert('Model reset to original state!');
};

const handleDragStart = (event) => {
  event.dataTransfer.setData('text/plain', '');
  emit('textureApplied', imagePreviewUrl.value);
};

const handleDragEnd = () => {
  // Có thể thêm logic nếu cần
};

// Preview text (optional)
const previewText = () => {
  // Add preview logic if needed
};

// Theo dõi customColor để áp dụng màu ngay khi thay đổi
watch(customColor, (newColor) => {
  applyBackgroundColor(newColor);
});

watch(() => props.model, (newModel) => {
  if (newModel) {
    console.log('Model ready for customization:', newModel);
  }
});

// Thêm sự kiện drop cho canvas
watch(() => props.renderer, (newRenderer) => {
  if (newRenderer) {
    const canvas = newRenderer.domElement;
    canvas.addEventListener('dragover', (event) => event.preventDefault());
    canvas.addEventListener('drop', handleTextDrop);
  }
});
</script>

<style scoped>
.sidebar-custom {
  width: 300px;
  height: 100vh;
  background: #2a3b5a;
  color: #ffffff;
  padding: 1.5rem;
  border-radius: 12px;
  box-shadow: 0 5px 20px rgba(0, 0, 0, 0.2);
  position: fixed;
  top: 0;
  left: 0;
  transform: none;
  z-index: 10;
  transition: all 0.3s ease;
}

.tabs {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  margin-bottom: 1.5rem;
}

.tab-btn {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.8rem 1rem;
  background: #1a263d;
  border: none;
  border-radius: 8px;
  color: #b0b8cc;
  font-size: 1rem;
  cursor: pointer;
  transition: all 0.3s ease;
}

.tab-btn:hover {
  background: #222f4e;
  color: #ffffff;
}

.tab-btn.active {
  background: #8bc34a;
  color: #ffffff;
}

.tab-content {
  padding: 1rem 0;
}

.upload-section {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.uploaded-file-info {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.image-preview {
  width: 100%;
  height: 250px;
  margin: 10px 0;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  background: #ffffff;
  cursor: grab;
}

.image-preview img {
  width: 100%;
  height: 100%;
  object-fit: contain;
  pointer-events: none;
}

.image-preview:hover {
  transform: scale(1.02);
  box-shadow: 0 6px 16px rgba(0, 0, 0, 0.2);
  transition: all 0.3s ease;
}

.error-text {
  color: #ff4444;
  font-size: 0.9rem;
  margin: 0;
}

.text-section {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.text-input {
  padding: 0.6rem 1rem;
  border: none;
  border-radius: 8px;
  background: #1a263d;
  color: #ffffff;
  font-size: 0.9rem;
}

.text-input::placeholder {
  color: #b0b8cc;
}

.text-input:focus {
  outline: none;
  box-shadow: 0 0 5px rgba(139, 195, 74, 0.5);
}

.text-options {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.text-options label {
  font-size: 0.9rem;
  color: #b0b8cc;
}

.text-options input[type="range"] {
  width: 100px;
}

.text-options input[type="color"] {
  width: 40px;
  height: 30px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  background: none;
  padding: 0;
}

.text-options input[type="color"]::-webkit-color-swatch {
  border: 2px solid #ffffff;
  border-radius: 5px;
}

.draggable-texts {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  margin-top: 1rem;
}

.draggable-text {
  padding: 0.5rem 1rem;
  background: #1a263d;
  border-radius: 8px;
  cursor: grab;
  transition: all 0.3s ease;
}

.draggable-text:hover {
  background: #222f4e;
  transform: scale(1.02);
}

.background-section {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
  background: linear-gradient(145deg, #1a263d, #2a3b5a);
  padding: 1rem;
  border-radius: 10px;
  box-shadow: inset 0 2px 10px rgba(0, 0, 0, 0.3);
  margin-bottom: 1.5rem;
  animation: fadeIn 0.3s ease-in-out;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(-10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.section-title {
  font-size: 1.1rem;
  margin: 0;
  color: #ffffff;
  text-align: center;
  text-transform: uppercase;
  letter-spacing: 1px;
}

.color-picker {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  justify-content: center;
}

.color-picker label {
  font-size: 0.9rem;
  color: #b0b8cc;
}

.color-picker input[type="color"] {
  width: 50px;
  height: 30px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  background: none;
  padding: 0;
}

.color-picker input[type="color"]::-webkit-color-swatch {
  border: 2px solid #ffffff;
  border-radius: 5px;
}

.color-picker input[type="color"]:hover {
  transform: scale(1.1);
  transition: all 0.3s ease;
}

.selected-color {
  font-size: 0.9rem;
  margin: 0;
  color: #ffffff;
  text-align: center;
  background: rgba(255, 255, 255, 0.1);
  padding: 0.5rem;
  border-radius: 5px;
}

.upload-btn {
  display: inline-block;
  padding: 0.8rem 2rem;
  background: #8bc34a;
  border-radius: 25px;
  text-align: center;
  cursor: pointer;
  font-weight: 600;
  transition: all 0.3s ease;
}

.upload-btn:hover {
  background: #7ab33f;
  transform: translateY(-2px);
  box-shadow: 0 5px 15px rgba(40, 167, 69, 0.3);
}

.apply-btn {
  padding: 0.6rem 1.5rem;
  background: #28a745;
  border: none;
  border-radius: 25px;
  color: #ffffff;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
}

.apply-btn:hover {
  background: #218838;
}

.apply-btn:disabled {
  background: #6c757d;
  cursor: not-allowed;
}

.reset-btn {
  padding: 0.6rem 1.5rem;
  background: #ff4444;
  border: none;
  border-radius: 25px;
  color: #ffffff;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
  margin-top: 1rem;
}

.reset-btn:hover {
  background: #cc0000;
}

@media (max-width: 768px) {
  .sidebar-custom {
    width: 100%;
    height: auto;
    position: relative;
    left: 0;
    padding: 10px;
  }
  
  .tabs {
    flex-direction: row;
    flex-wrap: wrap;
  }
  
  .tab-btn {
    flex: 1 1 auto;
    min-width: 120px;
  }
}
</style>

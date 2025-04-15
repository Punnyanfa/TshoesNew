<template>
  <div class="page-wrapper">
    <!-- Header Component -->
    <Header />

    <div class="content-wrapper">
      <main class="main-content">
        <div class="customizer-grid">
          <!-- Sidebar Custom Component -->
          <Sidebarcustom 
            class="sidebar-custom"
            :scene="scene" 
            :model="model" 
            :original-materials="originalMaterials"
            @texture-applied="handleTextureApplied"
            @text-applied="handleTextApplied" 
            @background-color-applied="handleBackgroundColorApplied"
          />

          <!-- Main Product Area (Center) -->
          <div class="product-area">
            <div class="three-canvas" 
                 @dragover.prevent="handleDragOver"
                 @dragleave.prevent="handleDragLeave"
                 @drop.prevent="handleDrop"
                 :class="{ 'drag-active': isDragging }">
              <!-- Khối 2D ban đầu -->
              <div v-if="!show3D" class="preview-block"></div>
              <!-- Canvas 3D chỉ hiển thị khi show3D = true -->
              <div v-show="show3D" ref="container" class="three-canvas-inner">
                <div v-if="!isModelLoaded" class="loading-overlay">
                  <span class="loading-text">Loading 3D Model...</span>
                </div>
                <div v-if="isDragging" class="drag-overlay">
                  <span>Thả ảnh vào đây để áp dụng texture</span>
                </div>
                <!-- Nút điều khiển animation -->
                <button v-if="isModelLoaded" class="animation-toggle-btn" @click="toggleAnimation">
                  {{ isAnimating ? 'Pause Animation' : 'Play Animation' }}
                </button>
              </div>
            </div>
          </div>

          <!-- Product Details and Controls (Right) -->
          <div class="details-section">
            <h1 class="product-title">Customizable Lightweight Breathable Running Sneakers</h1>
            <button class="customize-btn" @click="handleCustomize" :disabled="!isModelLoaded || !show3D">
              <span v-if="!isModelLoaded || !show3D">Loading...</span>
              <span v-else>Customize Now</span>
            </button>

            <!-- Product Details -->
            <div class="product-details">
              <p class="price">2,587,000₫</p>
              <div class="detail-row">
                <span>Color:</span>
                <span class="detail-value">White</span>
              </div>
              <div class="detail-row">
                <span>Size:</span>
                <select class="size-select">
                  <option>Women US 5.5 (EU 36)</option>
                  <option>Women US 6 (EU 37)</option>
                  <option>Women US 6.5 (EU 38)</option>
                  <option>Women US 7 (EU 39)</option>
                </select>
              </div>
              <div class="detail-row description-toggle" @click="toggleDescription">
                <span>Description</span>
                <button class="toggle-btn">{{ showDescription ? '▲' : '▼' }}</button>
              </div>
              <div v-if="showDescription" class="description-content">
                <p>Experience ultimate comfort and style with these lightweight, breathable running sneakers. Perfect for daily runs, workouts, or casual wear, featuring a durable sole and premium fabric for all-day support.</p>
              </div>
            </div>
          </div>
        </div>
      </main>
    </div>

    <!-- Footer Component -->
    <Footer />
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted, watch } from 'vue';
import * as THREE from 'three';
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls';
import Sidebarcustom from '~/components/sidebarcustom.vue';
import * as Popper from '@popperjs/core';
const { placements, createPopper } = Popper;

// Refs cho Three.js container và states
const container = ref(null);
const isModelLoaded = ref(false);
const showDescription = ref(false);
const isAnimating = ref(false);
const show3D = ref(true);
const isDragging = ref(false);
const currentTexture = ref(null);

// Khai báo các biến Three.js
let scene, camera, renderer, controls, mixer, model;
let originalMaterials = new Map();

const initThreeJS = () => {
  scene = new THREE.Scene();
  scene.background = new THREE.Color(0xffffff);

  camera = new THREE.PerspectiveCamera(
    75,
    window.innerWidth * 0.5 / (window.innerHeight * 0.8),
    0.1,
    1000
  );
  camera.position.set(0, 2, 5);

  // Cải thiện renderer
  renderer = new THREE.WebGLRenderer({
    antialias: true,
    alpha: true,
    preserveDrawingBuffer: true
  });
  renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2)); // Giới hạn pixel ratio để tối ưu hiệu suất
  renderer.setSize(window.innerWidth * 0.5, window.innerHeight * 0.8);
  renderer.outputEncoding = THREE.sRGBEncoding;
  renderer.toneMapping = THREE.ACESFilmicToneMapping;
  renderer.toneMappingExposure = 1.2; // Tăng độ sáng
  renderer.shadowMap.enabled = true;
  renderer.shadowMap.type = THREE.PCFSoftShadowMap;

  if (container.value && show3D.value) {
    container.value.appendChild(renderer.domElement);
  }

  // Thiết lập ánh sáng
  setupLighting();

  // Cải thiện controls
  controls = new OrbitControls(camera, renderer.domElement);
  controls.enableDamping = true;
  controls.dampingFactor = 0.05;
  controls.minDistance = 3;
  controls.maxDistance = 10;
  controls.maxPolarAngle = Math.PI / 1.5;
  controls.enablePan = false;

  // Tải model với chất lượng cao hơn
  const loader = new GLTFLoader();
  loader.load(
    '/Nike jordan.glb',
    (gltf) => {
      model = gltf.scene;
      
      // Cải thiện chất lượng model
      model.traverse((child) => {
        if (child.isMesh) {
          child.castShadow = true;
          child.receiveShadow = true;
          // Lưu material gốc
          originalMaterials.set(child, child.material.clone());
          // Tối ưu geometry
          if (child.geometry) {
            child.geometry.computeVertexNormals();
          }
        }
      });

      scene.add(model);
      model.scale.set(1, 1, 1);
      model.position.set(-1, 0, 0);
      
      isModelLoaded.value = true;
    },
    undefined,
    (error) => {
      console.error('❌ Error loading model:', error);
      isModelLoaded.value = false;
    }
  );
};

// Cập nhật animation loop
const animate = () => {
  requestAnimationFrame(animate);
  
  if (controls) controls.update();

  if (isAnimating.value && model) {
    if (mixer) {
      mixer.update(clock.getDelta());
    } else {
      model.rotation.y += 0.005; // Giảm tốc độ xoay
    }
  }

  if (renderer && scene && camera) {
    renderer.render(scene, camera);
  }
};

// Khi mounted
onMounted(() => {
  initThreeJS();
  animate();
});

// Cleanup khi unmounted
onUnmounted(() => {
  if (renderer) {
    renderer.dispose();
    scene.clear();
  }
  window.removeEventListener('resize', resizeHandler);
});

// Xử lý resize
const resizeHandler = () => {
  camera.aspect = window.innerWidth * 0.5 / (window.innerHeight * 0.8);
  camera.updateProjectionMatrix();
  renderer.setSize(window.innerWidth * 0.5, window.innerHeight * 0.8);
};
window.addEventListener('resize', resizeHandler);

// Theo dõi show3D
watch(show3D, (newVal) => {
  if (newVal && container.value && renderer) {
    container.value.appendChild(renderer.domElement);
  }
});

// Điều khiển animation
const toggleAnimation = () => {
  if (!isModelLoaded.value) return;
  isAnimating.value = !isAnimating.value;

  if (mixer) {
    const action = mixer.clipAction(mixer.getRoot().animations[0]);
    if (isAnimating.value) {
      action.play();
    } else {
      action.stop();
    }
  }
};

// Handle customize button click
const handleCustomize = () => {
  if (isModelLoaded.value && show3D.value) {
    alert('Customize functionality will open here! (Implement your customization logic.)');
  }
};

// Handle texture applied event from SidebarCustom
const handleTextureApplied = (textureUrl) => {
  currentTexture.value = textureUrl;
};

// Handle text applied event from SidebarCustom
const handleTextApplied = (text) => {
  console.log('Text applied to model:', text);
};

// Handle background color applied event from SidebarCustom
const handleBackgroundColorApplied = (color) => {
  console.log('Background color applied to model:', color);
};

// Toggle description visibility
const toggleDescription = () => {
  showDescription.value = !showDescription.value;
};

// Thêm các hàm xử lý drag & drop
const handleDragOver = (event) => {
  event.preventDefault();
  isDragging.value = true;
};

const handleDragLeave = () => {
  isDragging.value = false;
};

// Cập nhật hàm applyTextureToObject
const applyTextureToObject = (object, textureUrl) => {
  if (!textureUrl) return;

  const textureLoader = new THREE.TextureLoader();
  textureLoader.load(textureUrl, (texture) => {
    // Cải thiện chất lượng texture
    texture.anisotropy = renderer.capabilities.getMaxAnisotropy();
    texture.encoding = THREE.sRGBEncoding;
    texture.needsUpdate = true;

    // Đảm bảo texture hiển thị đúng tỷ lệ và không bị lặp
    texture.wrapS = THREE.ClampToEdgeWrapping;
    texture.wrapT = THREE.ClampToEdgeWrapping;
    texture.repeat.set(1, 1);
    texture.offset.set(0, 0);
    
    // Tối ưu filter cho texture rõ nét
    texture.minFilter = THREE.LinearFilter;
    texture.magFilter = THREE.LinearFilter;
    texture.generateMipmaps = false; // Tắt mipmaps để tránh làm mờ texture

    // Tạo material mới với các thuộc tính tối ưu
    const newMaterial = new THREE.MeshStandardMaterial({
      map: texture,
      metalness: 0, // Giảm hoàn toàn độ kim loại
      roughness: 0.5,
      transparent: true,
      opacity: 1,
      side: THREE.DoubleSide,
      envMapIntensity: 1,
    });

    // Áp dụng material
    if (object.material) {
      object.material.dispose();
    }
    object.material = newMaterial;

    // Điều chỉnh UV mapping để texture hiển thị đúng
    if (object.geometry) {
      const uvAttribute = object.geometry.attributes.uv;
      if (uvAttribute) {
        // Reset UV coordinates về mặc định
        for (let i = 0; i < uvAttribute.count; i++) {
          uvAttribute.setXY(
            i,
            uvAttribute.getX(i),
            uvAttribute.getY(i)
          );
        }
        uvAttribute.needsUpdate = true;
      }
      object.geometry.computeVertexNormals();
    }

    // Đảm bảo cập nhật material
    object.material.needsUpdate = true;
  });
};

// Cập nhật lighting setup để texture hiển thị rõ hơn
const setupLighting = () => {
  // Xóa ánh sáng cũ
  scene.children.forEach(child => {
    if (child.isLight) scene.remove(child);
  });

  // Ánh sáng môi trường mạnh hơn
  const ambientLight = new THREE.AmbientLight(0xffffff, 1.5);
  scene.add(ambientLight);

  // Ánh sáng chính từ phía trước
  const mainLight = new THREE.DirectionalLight(0xffffff, 1.5);
  mainLight.position.set(0, 5, 10);
  scene.add(mainLight);

  // Ánh sáng phụ từ các hướng khác
  const backLight = new THREE.DirectionalLight(0xffffff, 1);
  backLight.position.set(0, 5, -10);
  scene.add(backLight);

  const leftLight = new THREE.DirectionalLight(0xffffff, 1);
  leftLight.position.set(-10, 5, 0);
  scene.add(leftLight);

  const rightLight = new THREE.DirectionalLight(0xffffff, 1);
  rightLight.position.set(10, 5, 0);
  scene.add(rightLight);
};

// Thêm hàm xử lý khi texture được load
const handleTextureLoaded = (texture) => {
  texture.flipY = false; // Thử đảo ngược texture nếu bị lộn ngược
};

// Cập nhật hàm handleDrop
const handleDrop = async (event) => {
  isDragging.value = false;
  
  if (!model || !currentTexture.value) return;

  const rect = event.target.getBoundingClientRect();
  const x = ((event.clientX - rect.left) / rect.width) * 2 - 1;
  const y = -((event.clientY - rect.top) / rect.height) * 2 + 1;

  const raycaster = new THREE.Raycaster();
  const mouse = new THREE.Vector2(x, y);
  raycaster.setFromCamera(mouse, camera);

  const intersects = raycaster.intersectObject(model, true);
  
  if (intersects.length > 0) {
    const intersectedObject = intersects[0].object;
    
    // Tạm dừng animation
    const wasAnimating = isAnimating.value;
    isAnimating.value = false;

    try {
      // Lưu trữ material gốc trước khi áp dụng texture mới
      if (!originalMaterials.has(intersectedObject)) {
        originalMaterials.set(intersectedObject, intersectedObject.material.clone());
      }

      await applyTextureToObject(intersectedObject, currentTexture.value);
      
      // Khôi phục animation
      isAnimating.value = wasAnimating;
    } catch (error) {
      console.error('Lỗi khi áp dụng texture:', error);
      alert('Có lỗi xảy ra khi áp dụng texture.');
    }
  }
};
</script>

<style scoped>
/* General Layout */
.page-wrapper {
  min-height: 100vh;
  background: linear-gradient(135deg, #f5f7fa 0%, #e0e8f5 100%);
}

/* Main Content */
.main-content {
  flex: 1;
  padding: 2rem;
  max-width: 1400px;
  margin: 0 auto; /* Thêm margin auto để căn giữa */
}

/* Customizer Grid */
.customizer-grid {
  display: flex;
  gap: 2rem;
  align-items: flex-start;
  width: 100%;
  position: relative;
}

/* Sidebar Custom */
.sidebar-custom {
  flex: 1;
  background: #ffffff;
  box-shadow: 2px 0 10px rgba(0, 0, 0, 0.1);
  border-radius: 12px;
  padding: 1.5rem;
  position: relative;
  height: fit-content; /* Chiều cao theo nội dung */
  max-height: 650px; /* Giới hạn chiều cao tối đa */
  overflow-y: auto; /* Cho phép scroll nếu nội dung dài */
}

/* Product Area (Center) */
.product-area {
  flex: 1;
  display: flex;
  flex-direction: column;
  align-items: center;
  min-height: 500px; /* Đảm bảo khu vực hiển thị 3D có đủ chiều cao */
}

.three-canvas {
  width: 100%;
  max-width: 500px;
  height: 400px;
  background-color: #ffffff;
  border-radius: 12px;
  box-shadow: 0 5px 20px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  position: relative;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.three-canvas:hover {
  transform: scale(1.02);
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.15);
}

.three-canvas-inner {
  width: 100%;
  height: 100%;
  position: relative;
}

/* Loading Overlay */
.loading-overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(255, 255, 255, 0.9);
  display: flex;
  justify-content: center;
  align-items: center;
  border-radius: 12px;
}

.loading-text {
  font-size: 1.2rem;
  color: #1a3c6c;
  font-weight: 600;
  animation: pulse 1.5s infinite;
}

@keyframes pulse {
  0% { opacity: 1; }
  50% { opacity: 0.7; }
  100% { opacity: 1; }
}

/* Animation Toggle Button */
.animation-toggle-btn {
  position: absolute;
  bottom: 10px;
  left: 10px;
  padding: 0.5rem 1rem;
  font-size: 0.9rem;
  font-weight: 600;
  color: #ffffff;
  background-color: #1a3c6c;
  border: none;
  border-radius: 20px;
  cursor: pointer;
  transition: background-color 0.3s ease, transform 0.2s ease;
}

.animation-toggle-btn:hover {
  background-color: #2858a0;
  transform: scale(1.05);
}

.animation-toggle-btn:active {
  transform: scale(0.95);
}

/* Details Section (Right) */
.details-section {
  flex: 1;
  background: #ffffff;
  padding: 1.5rem;
  border-radius: 12px;
  box-shadow: 0 5px 20px rgba(0, 0, 0, 0.1);
  height: fit-content; /* Chiều cao theo nội dung */
}

.product-title {
  font-size: 1.8rem;
  font-weight: 700;
  color: #1a3c6c;
  margin: 0;
  text-transform: uppercase;
  letter-spacing: 1px;
  text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.05);
}

.customize-btn {
  padding: 1rem 2.5rem;
  font-size: 1.1rem;
  font-weight: 700;
  color: #ffffff;
  background-color: #8bc34a;
  border: none;
  border-radius: 25px;
  cursor: pointer;
  transition: background-color 0.3s ease, transform 0.2s ease, box-shadow 0.3s ease;
  box-shadow: 0 5px 15px rgba(40, 167, 69, 0.3);
}

.customize-btn:hover:not(:disabled) {
  background-color: #8bc34a;
  transform: translateY(-3px);
  box-shadow: 0 8px 20px rgba(40, 167, 69, 0.4);
}

.customize-btn:active:not(:disabled) {
  transform: translateY(0);
  box-shadow: 0 3px 10px rgba(40, 167, 69, 0.3);
}

.customize-btn:disabled {
  background-color: #a0d9b4;
  cursor: not-allowed;
  opacity: 0.7;
}

/* Product Details */
.product-details {
  margin-top: 1.5rem;
}

.price {
  font-size: 1.4rem;
  font-weight: 700;
  color: #1a3c6c;
  margin-bottom: 1rem;
  text-align: right;
}

.detail-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
  font-size: 1rem;
  color: #444;
  transition: color 0.3s ease;
}

.detail-row:hover {
  color: #1a3c6c;
}

.detail-value {
  font-weight: 500;
  color: #333;
}

.size-select {
  padding: 0.5rem;
  border: 2px solid #ccc;
  border-radius: 6px;
  font-size: 1rem;
  background-color: #fff;
  transition: border-color 0.3s ease, box-shadow 0.3s ease;
}

.size-select:focus {
  outline: none;
  border-color: #28a745;
  box-shadow: 0 0 5px rgba(40, 167, 69, 0.3);
}

.description-toggle {
  cursor: pointer;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.8rem 0;
  border-top: 2px solid #eee;
  transition: color 0.3s ease, transform 0.3s ease;
}

.description-toggle:hover {
  color: #1a3c6c;
  transform: translateX(5px);
}

.toggle-btn {
  background: none;
  border: none;
  font-size: 1rem;
  color: #666;
  cursor: pointer;
  transition: color 0.3s ease;
}

.toggle-btn:hover {
  color: #1a3c6c;
}

.description-content {
  margin-top: 1rem;
  padding: 1rem;
  background: #f9f9f9;
  border-radius: 8px;
  box-shadow: inset 0 2px 5px rgba(0, 0, 0, 0.05);
  font-size: 0.9rem;
  color: #555;
  line-height: 1.6;
  animation: fadeIn 0.5s ease-in;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(-10px); }
  to { opacity: 1; transform: translateY(0); }
}

/* Responsive Design */
@media (max-width: 1200px) {
  .customizer-grid {
    flex-direction: column;
  }

  .sidebar-custom,
  .product-area,
  .details-section {
    width: 100%;
  }
}

@media (max-width: 1024px) {
  .customizer-grid {
    flex-direction: column; /* Stack vertically on smaller screens */
    gap: 1.5rem;
  }

  .sidebar-custom,
  .product-area,
  .details-section {
    width: 100%;
  }

  .three-canvas {
    max-width: 100%;
    height: 350px;
  }

  .details-section {
    padding: 1.5rem;
  }

  .product-title {
    font-size: 1.6rem;
  }
}

@media (max-width: 768px) {
  .three-canvas {
    height: 300px;
  }

  .product-title {
    font-size: 1.4rem;
  }

  .customize-btn {
    padding: 0.8rem 2rem;
    font-size: 1rem;
  }

  .price {
    font-size: 1.2rem;
  }

  .detail-row {
    font-size: 0.9rem;
  }

  .animation-toggle-btn {
    font-size: 0.8rem;
    padding: 0.4rem 0.8rem;
  }
}

/* Thêm styles mới */
.content-wrapper {
  min-height: calc(100vh - 120px); /* Điều chỉnh theo chiều cao của header và footer */
}

/* Thêm styles mới cho drag & drop */
.three-canvas {
  position: relative;
}

.drag-active {
  border: 2px dashed #4CAF50;
  background-color: rgba(76, 175, 80, 0.05);
}

.drag-overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: rgba(255, 255, 255, 0.8);
  z-index: 10;
  pointer-events: none;
}

.drag-overlay span {
  padding: 15px 25px;
  background-color: #4CAF50;
  color: white;
  border-radius: 8px;
  font-weight: bold;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.three-canvas-inner {
  position: relative;
  width: 100%;
  height: 100%;
}

/* Thêm style cho loading message */
.texture-loading-message {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  background-color: rgba(0, 0, 0, 0.7);
  color: white;
  padding: 10px 20px;
  border-radius: 20px;
  z-index: 1000;
}

/* Điều chỉnh style cho preview texture */
.image-preview {
  max-width: 150px; /* Giới hạn kích thước tối đa của preview */
  max-height: 150px;
  margin: 0 auto;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.image-preview img {
  width: 100%;
  height: 100%;
  object-fit: contain; /* Giữ tỷ lệ ảnh */
  border-radius: 8px;
}

/* Animation cho texture loading */
@keyframes fadeInOut {
  0% { opacity: 0; }
  50% { opacity: 1; }
  100% { opacity: 0; }
}

.texture-loading-message {
  animation: fadeInOut 1.5s infinite;
}
</style>

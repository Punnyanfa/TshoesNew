<template>
  <div class="page-wrapper">
    <!-- Header Component -->
    <Header />

    <!-- Sidebar Custom Component -->
    <SidebarCustom :scene="scene" :model="model" @texture-applied="handleTextureApplied" />

    <!-- Main Content Section -->
    <main class="main-content">
      <div class="customizer-grid">
        <!-- Main Product Area (Left) -->
        <div class="product-area">
          <div class="three-canvas">
            <!-- Khối 2D ban đầu -->
            <div v-if="!show3D" class="preview-block"></div>
            <!-- Canvas 3D chỉ hiển thị khi show3D = true -->
            <div v-show="show3D" ref="container" class="three-canvas-inner">
              <div v-if="!isModelLoaded" class="loading-overlay">
                <span class="loading-text">Loading 3D Model...</span>
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

    <!-- Footer Component -->
    <Footer />
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted, watch } from 'vue';
import * as THREE from 'three';
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls';
import SidebarCustom from '@/components/SidebarCustom.vue';

// Refs cho Three.js container và states
const container = ref(null);
const isModelLoaded = ref(false);
const showDescription = ref(false);
const isAnimating = ref(false);
const show3D = ref(true);

// Khai báo các biến Three.js
let scene, camera, renderer, controls, mixer, model;

const initThreeJS = () => {
  // Khởi tạo scene
  scene = new THREE.Scene();
  scene.background = new THREE.Color(0xffffff);

  // Khởi tạo camera
  camera = new THREE.PerspectiveCamera(75, window.innerWidth * 0.5 / (window.innerHeight * 0.8), 0.1, 1000);
  camera.position.set(0, 2, 5);

  // Khởi tạo renderer
  renderer = new THREE.WebGLRenderer({ antialias: true });
  renderer.setSize(window.innerWidth * 0.5, window.innerHeight * 0.8);
  if (container.value && show3D.value) {
    container.value.appendChild(renderer.domElement);
  }

  // Thêm ánh sáng
  const ambientLight = new THREE.AmbientLight(0xffffff, 0.6);
  scene.add(ambientLight);
  const directionalLight = new THREE.DirectionalLight(0xffffff, 0.8);
  directionalLight.position.set(5, 5, 5);
  scene.add(directionalLight);

  // Khởi tạo controls
  controls = new OrbitControls(camera, renderer.domElement);
  controls.enableDamping = true;
  controls.dampingFactor = 0.05;

  // Tải model 3D
  const loader = new GLTFLoader();
  loader.load(
    '/Nike jordan4.glb',
    (gltf) => {
      console.log('✅ Model loaded successfully:', gltf);
      model = gltf.scene;
      scene.add(model);

      // Điều chỉnh vị trí và tỉ lệ của model
      model.scale.set(1, 1, 1);
      model.position.set(-1, 0, 0);

      // Khởi tạo AnimationMixer nếu có animation
      if (gltf.animations && gltf.animations.length > 0) {
        mixer = new THREE.AnimationMixer(model);
        const action = mixer.clipAction(gltf.animations[0]);
        action.setLoop(THREE.LoopRepeat); // Lặp lại animation
        action.play(); // Chạy ngay khi tải xong
        isAnimating.value = true;
      } else {
        // Nếu không có animation, xoay mặc định
        isAnimating.value = true;
      }

      isModelLoaded.value = true;
    },
    (xhr) => {
      console.log(`Loading progress: ${(xhr.loaded / xhr.total) * 100}%`);
    },
    (error) => {
      console.error('❌ Error loading model:', error);
      isModelLoaded.value = false;
    }
  );
};

// Animation loop
const clock = new THREE.Clock();
const animate = () => {
  requestAnimationFrame(animate);
  controls.update();

  if (isAnimating.value) {
    if (mixer) {
      mixer.update(clock.getDelta());
    } else if (model) {
      // Animation mặc định: xoay model
      model.rotation.y += 0.01;
    }
  }

  renderer.render(scene, camera);
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
const handleTextureApplied = (texture) => {
  console.log('Texture applied to model:', texture);
  // Additional logic if needed after texture is applied
};

// Toggle description visibility
const toggleDescription = () => {
  showDescription.value = !showDescription.value;
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
  padding: 2rem;
  max-width: 1400px; /* Increased max-width for better spacing */
  margin: 0 0 0 500px;
}

/* Customizer Grid */
.customizer-grid {
  display: grid;
  grid-template-columns: 1fr 2fr 1fr; /* Sidebar | Product Area | Details */
  gap: 2rem;
  align-items: start;
}

/* Sidebar Section (Left) */
.sidebar-section {
  background: #ffffff;
  padding: 1.5rem;
  border-radius: 12px;
  box-shadow: 0 5px 20px rgba(0, 0, 0, 0.1);
  max-width: 300px; /* Limit sidebar width */
}

/* Product Area (Center) */
.product-area {
  display: flex;
  flex-direction: column;
  align-items: center;
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
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
  background: #ffffff;
  padding: 2rem;
  border-radius: 12px;
  box-shadow: 0 5px 20px rgba(0, 0, 0, 0.1);
  transition: transform 0.3s ease;
  max-width: 400px; /* Limit details section width */
}

.details-section:hover {
  transform: translateY(-5px);
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
@media (max-width: 1024px) {
  .customizer-grid {
    grid-template-columns: 1fr; /* Stack vertically on smaller screens */
    gap: 1.5rem;
  }

  .sidebar-section,
  .product-area,
  .details-section {
    max-width: 100%; /* Full width on smaller screens */
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
</style>
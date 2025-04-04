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
            @reset="handleReset"
            @image-selected="handleImageSelected"
            @addImageTo2D="handleAddImageTo2D"
            @text-added-to-canvas="handleTextAddedToCanvas"
            @text-style-updated="handleTextStyleUpdated"
            ref="sidebarRef"
          />

          <!-- Main Product Area (Center) -->
          <div class="product-area">
            <!-- 2D PNG Frame (Moved to top) -->
            <div class="sneaker-2d-section">
              <div ref="stageContainer" class="canvas-container"></div>
            </div>

            <div class="product-preview">
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
                    <div class="loading-spinner"></div>
                    <span class="loading-text">Loading 3D Model...</span>
                  </div>
                  <div v-if="isDragging" class="drag-overlay">
                    <div class="drop-zone">
                      <i class="upload-icon">📁</i>
                      <span>Thả ảnh vào đây để áp dụng texture</span>
                    </div>
                  </div>
                  <!-- Nút điều khiển animation -->
                  <button v-if="isModelLoaded" class="animation-toggle-btn" @click="toggleAnimation">
                    <i :class="['animation-icon', isAnimating ? 'pause' : 'play']"></i>
                    {{ isAnimating ? 'Pause Animation' : 'Play Animation' }}
                  </button>
                </div>
              </div>
            </div>
          </div>

          <!-- Product Details and Controls (Right) -->
          <div class="edit-tools-section">
            <div v-if="!currentTexture && !selectedImage" class="empty-state">
              <div class="empty-state-icon">🖱️</div>
              <div class="empty-state-text">Click image or Text in the canvas to edit</div>
            </div>
            <template v-else>
              <!-- Công cụ chỉnh sửa text -->
              <div v-if="selectedImage && selectedImage.getType() === 'Text'" class="tool-group">
                <div class="tool-item">
                  <button class="tool-btn remove-btn" @click="handleRemoveText">
                    <i class="tool-icon">🗑️</i>
                    Remove
                  </button>
                </div>
                <div class="tool-item">
                  <button class="tool-btn duplicate-btn" @click="handleDuplicateText">
                    <i class="tool-icon">📋</i>
                    Duplicate
                  </button>
                </div>
                <div class="tool-item">
                  <button class="tool-btn" @click="handleEditText">
                    <i class="tool-icon">✏️</i>
                    Edit Text
                  </button>
                </div>
                <div class="tool-item">
                  <div class="tool-label">Font</div>
                  <div class="font-display">ABeeZee</div>
                </div>
                <div class="tool-item">
                  <div class="tool-label">Color</div>
                  <div class="color-display" @click="openColorPicker" :style="{ backgroundColor: selectedTextColor }"></div>
                  <!-- Color Picker Modal -->
                  <div v-if="showColorPicker" class="color-picker-overlay" @click="closeColorPicker">
                    <div class="color-picker-content" @click.stop>
                      <div class="color-picker-header">
                        <span>Chọn màu</span>
                        <button class="close-btn" @click="closeColorPicker">&times;</button>
                      </div>
                      <div class="color-picker-body">
                        <div class="color-preview" :style="{ backgroundColor: selectedTextColor }"></div>
                        <input 
                          type="color" 
                          v-model="selectedTextColor" 
                          @input="handleColorChange"
                          class="color-picker-input"
                        />
                      </div>
                      <div class="color-picker-footer">
                        <button class="color-picker-btn cancel" @click="closeColorPicker">Hủy</button>
                        <button class="color-picker-btn apply" @click="applyColor">Áp dụng</button>
                      </div>
                    </div>
                  </div>
                </div>
                <div class="tool-item">
                  <div class="tool-label">Font Size</div>
                  <div class="size-display">{{ selectedTextSize }}</div>
                </div>
                <div class="tool-item">
                  <div class="tool-label">Curvature</div>
                  <div class="range-control">
                    <input 
                      type="range" 
                      v-model="textCurvature" 
                      @input="updateTextCurvature"
                      min="0"
                      max="100"
                      class="range-slider"
                    />
                    <span class="range-value">{{ textCurvature }}</span>
                  </div>
                </div>
                <div class="tool-item">
                  <div class="tool-actions">
                    <button class="tool-btn" @click="handleTextFill">
                      <i class="tool-icon">🖼️</i>
                      Fill
                    </button>
                    <button class="tool-btn" @click="handleTextFit">
                      <i class="tool-icon">📏</i>
                      Fit
                    </button>
                  </div>
                </div>
                <div class="tool-item">
                  <div class="tool-label">Scale</div>
                  <div class="scale-controls">
                    <button class="tool-btn" @click="handleTextScale(1)">+</button>
                    <button class="tool-btn" @click="handleTextScale(-1)">-</button>
                    <button class="tool-btn" @click="handleTextScaleInput">
                      <i class="tool-icon">✏️</i>
                    </button>
                  </div>
                </div>
                <div class="tool-item">
                  <div class="tool-label">Rotate</div>
                  <div class="rotate-controls">
                    <button class="tool-btn" @click="handleTextRotate(-90)">
                      <i class="tool-icon">↺</i>
                    </button>
                    <button class="tool-btn" @click="handleTextRotate(90)">
                      <i class="tool-icon">↻</i>
                    </button>
                    <input type="text" class="rotate-input" v-model="textRotation" @input="handleTextRotateInput" />
                  </div>
                </div>
                <div class="tool-item">
                  <div class="tool-label">Move</div>
                  <div class="move-controls">
                    <button class="tool-btn" @click="handleTextMove('up')">↑</button>
                    <button class="tool-btn" @click="handleTextMove('down')">↓</button>
                    <button class="tool-btn" @click="handleTextMove('left')">←</button>
                    <button class="tool-btn" @click="handleTextMove('right')">→</button>
                  </div>
                </div>
                <div class="tool-item">
                  <div class="tool-label">Flip</div>
                  <div class="flip-controls">
                    <button class="tool-btn" @click="handleTextFlip('horizontal')">↔</button>
                    <button class="tool-btn" @click="handleTextFlip('vertical')">↕</button>
                  </div>
                </div>
              </div>
              
              <!-- Công cụ chỉnh sửa hình ảnh (giữ nguyên) -->
              <div v-else class="tool-group">
                <div class="tool-item">
                  <span class="tool-label">Print Quality:</span>
                  <span class="tool-value good">Good (DPI: 96)</span>
                </div>
                <div class="tool-actions">
                  <button class="tool-btn remove-btn" @click="handleRemove">
                    <i class="tool-icon">🗑️</i>
                    Remove
                  </button>
                  <button class="tool-btn duplicate-btn" @click="handleDuplicate">
                    <i class="tool-icon">📋</i>
                    Duplicate
                  </button>
                </div>
              </div>
              <div class="tool-group">
                <div class="tool-actions">
                  <button class="tool-btn" @click="handleFill">
                    <i class="tool-icon">🖼️</i>
                    Fill
                  </button>
                  <button class="tool-btn" @click="handleFit">
                    <i class="tool-icon">📏</i>
                    Fit
                  </button>
                </div>
              </div>
              <div class="tool-group">
                <div class="tool-label">Scale</div>
                <div class="scale-controls">
                  <button class="tool-btn" @click="handleScale(1)">
                    <i class="tool-icon">+</i>
                  </button>
                  <button class="tool-btn" @click="handleScale(-1)">
                    <i class="tool-icon">-</i>
                  </button>
                  <button class="tool-btn" @click="handleScaleInput">
                    <i class="tool-icon">✏️</i>
                  </button>
                </div>
              </div>
              <div class="tool-group">
                <div class="tool-label">Rotate</div>
                <div class="rotate-controls">
                  <button class="tool-btn" @click="handleRotate(-90)">
                    <i class="tool-icon">↺</i>
                  </button>
                  <button class="tool-btn" @click="handleRotate(90)">
                    <i class="tool-icon">↻</i>
                  </button>
                  <input type="text" class="rotate-input" value="0" @input="handleRotateInput" />
                </div>
              </div>
              <div class="tool-group">
                <div class="tool-label">Move</div>
                <div class="move-controls">
                  <button class="tool-btn" @click="handleMove('up')">
                    <i class="tool-icon">↑</i>
                  </button>
                  <button class="tool-btn" @click="handleMove('down')">
                    <i class="tool-icon">↓</i>
                  </button>
                  <button class="tool-btn" @click="handleMove('left')">
                    <i class="tool-icon">←</i>
                  </button>
                  <button class="tool-btn" @click="handleMove('right')">
                    <i class="tool-icon">→</i>
                  </button>
                </div>
              </div>
              <div class="tool-group">
                <div class="tool-label">Flip</div>
                <div class="flip-controls">
                  <button class="tool-btn" @click="handleFlip('horizontal')">
                    <i class="tool-icon">↔️</i>
                  </button>
                  <button class="tool-btn" @click="handleFlip('vertical')">
                    <i class="tool-icon">↕️</i>
                  </button>
                </div>
              </div>
            </template>
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
// import Canvas2D from '@/components/Canvas2D.vue';
import sneaker2DImage from '/JordanUV.png';
import Konva from 'konva';
import Sidebarcustom from '~/components/sidebarcustom.vue';

// Refs cho Three.js container và states
const container = ref(null);
const isModelLoaded = ref(false);
const showDescription = ref(false);
const isAnimating = ref(false);
const show3D = ref(true);
const isDragging = ref(false);
const currentTexture = ref(null);
const selectedImage = ref(null);
const stage = ref(null);
const layer = ref(null);
const stageConfig = ref({
  width: 500,
  height: 400
});

// Khai báo các biến Three.js
let scene, camera, renderer, controls, mixer, model;
let originalMaterials = new Map();

// Thêm vào phần khai báo state
const editingState = ref({
  rotation: 0,
  isEditing: false
});

// Thêm refs cho Konva
const stageContainer = ref(null);
const backgroundImage = ref(null);
const uploadedImages = ref([]);
const textObjects = ref([]); // Thêm mảng để lưu trữ các đối tượng text

const imageConfig = ref({
  x: 0,
  y: 0,
  image: sneaker2DImage,
  width: stageConfig.value.width,
  height: stageConfig.value.height,
  draggable: true
});

// Thêm vào phần khai báo state
const selectedTextColor = ref('#000000');
const selectedTextSize = ref(24);
const selectedTextFont = ref('Arial');
const selectedTextBold = ref(false);
const selectedTextItalic = ref(false);
const selectedTextUnderline = ref(false);
const selectedTextAlign = ref('center');

// Thêm các biến mới cho text
const textCurvature = ref(0);
const textRotation = ref(0);

// Thêm vào phần khai báo state
const showColorPicker = ref(false);

// Thêm biến để lưu màu tạm thời
const tempColor = ref('');

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

// Khởi tạo Konva stage
const initKonva = () => {
  if (!stageContainer.value) return;

  // Tạo stage với kích thước mới
  stage.value = new Konva.Stage({
    container: stageContainer.value,
    width: stageConfig.value.width,
    height: stageConfig.value.height
  });

  // Tạo layer
  layer.value = new Konva.Layer();
  stage.value.add(layer.value);

  // Tạo background image với kích thước mới
  const image = new Image();
  image.src = sneaker2DImage;
  image.onload = () => {
    // Tính toán kích thước phù hợp cho background
    const maxWidth = stageConfig.value.width;
    const maxHeight = stageConfig.value.height;
    
    let width = image.width;
    let height = image.height;
    
    // Tính tỷ lệ khung hình
    const aspectRatio = width / height;
    
    // Điều chỉnh kích thước nếu quá lớn
    if (width > maxWidth) {
      width = maxWidth;
      height = width / aspectRatio;
    }
    if (height > maxHeight) {
      height = maxHeight;
      width = height * aspectRatio;
    }

    // Tính toán vị trí để căn giữa
    const x = (stageConfig.value.width - width) / 2;
    const y = (stageConfig.value.height - height) / 2;

    // Tạo background image
    backgroundImage.value = new Konva.Image({
      x: x,
      y: y,
      image: image,
      width: width,
      height: height,
      listening: false // Tắt sự kiện chuột cho background
    });

    // Thêm background vào layer
    layer.value.add(backgroundImage.value);
    
    // Vẽ lại layer
    layer.value.draw();
  };

  // Xử lý lỗi khi load ảnh
  image.onerror = (error) => {
    console.error('Lỗi khi load ảnh background:', error);
  };
};

// Thêm onMounted hook
onMounted(() => {
  initThreeJS();
  animate();
  initKonva();
});

// Cleanup khi unmounted
onUnmounted(() => {
  if (renderer) {
    renderer.dispose();
    scene.clear();
  }
  window.removeEventListener('resize', resizeHandler);
  if (stage.value) {
    stage.value.destroy();
  }
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
  console.log('Background color applied to canvas 2D:', color);
  
  // Thay đổi màu nền của canvas 2D
  if (stage.value && layer.value) {
    // Tạo một hình chữ nhật làm nền với màu đã chọn
    const backgroundRect = new Konva.Rect({
      x: 0,
      y: 0,
      width: stageConfig.value.width,
      height: stageConfig.value.height,
      fill: color,
      listening: false,
      name: 'background-rect'
    });
    
    // Xóa hình chữ nhật nền cũ nếu có
    const existingBackground = layer.value.find('.background-rect');
    if (existingBackground.length > 0) {
      existingBackground[0].destroy();
    }
    
    // Thêm hình chữ nhật mới vào layer và đặt nó ở dưới cùng
    layer.value.add(backgroundRect);
    backgroundRect.moveToBottom();
    
    // Đảm bảo backgroundImage nằm trên background rect
    if (backgroundImage.value) {
      backgroundImage.value.moveUp();
    }
    
    // Vẽ lại layer
    layer.value.draw();
  }
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

// Cập nhật hàm handleReset
const handleReset = () => {
  // Reset currentTexture để ẩn các công cụ
  currentTexture.value = null;
  
  // Reset trạng thái chỉnh sửa
  editingState.value = {
    rotation: 0,
    isEditing: false
  };
  
  // Reset các giá trị khác về mặc định
  if (model) {
    model.traverse((child) => {
      if (child.isMesh && originalMaterials.has(child)) {
        // Khôi phục material gốc
        child.material.dispose();
        child.material = originalMaterials.get(child).clone();
      }
    });
  }

  // Reset canvas 2D
  if (stage.value && layer.value) {
    // Xóa tất cả các hình ảnh và text đã upload
    const children = layer.value.getChildren();
    children.forEach(child => {
      if (child.getClassName() === 'Image' || child.getClassName() === 'Text') {
        child.destroy();
      }
    });
    
    layer.value.draw();
  }
  
  // Reset mảng uploaded images và text objects
  uploadedImages.value = [];
  textObjects.value = [];
};

// Thêm watch cho currentTexture
watch(currentTexture, (newVal) => {
  if (!newVal) {
    // Reset các giá trị của công cụ chỉnh sửa về mặc định
    if (document.querySelector('.rotate-input')) {
      document.querySelector('.rotate-input').value = '0';
    }
  }
});

// Thêm các hàm xử lý cho các nút
const handleRemove = () => {
  if (layer.value) {
    // Xóa tất cả các node trên layer trừ backgroundImage
    const children = [...layer.value.getChildren()];
    children.forEach(child => {
      // Kiểm tra nếu không phải là backgroundImage thì xóa
      if (child !== backgroundImage.value) {
        child.destroy();
      }
    });
    
    // Reset các giá trị
    selectedImage.value = null;
    currentTexture.value = null;
    textObjects.value = [];
    
    // Vẽ lại layer chỉ với backgroundImage
    layer.value.draw();
  }
};

const handleDuplicate = () => {
  if (selectedImage.value) {
    const imageData = {
      url: selectedImage.value.image().src,
      timestamp: Date.now()
    };
    handleAddImageTo2D(imageData);
  }
};

const handleFill = () => {
  if (selectedImage.value) {
    const stageWidth = stageConfig.value.width;
    const stageHeight = stageConfig.value.height;
    selectedImage.value.width(stageWidth);
    selectedImage.value.height(stageHeight);
    selectedImage.value.x(0);
    selectedImage.value.y(0);
    layer.value.draw();
  }
};

const handleFit = () => {
  if (selectedImage.value) {
    const image = selectedImage.value.image();
    const stageWidth = stageConfig.value.width;
    const stageHeight = stageConfig.value.height;
    
    const imageAspect = image.width / image.height;
    const stageAspect = stageWidth / stageHeight;
    
    let width, height;
    if (imageAspect > stageAspect) {
      width = stageWidth;
      height = stageWidth / imageAspect;
    } else {
      height = stageHeight;
      width = stageHeight * imageAspect;
    }
    
    selectedImage.value.width(width);
    selectedImage.value.height(height);
    selectedImage.value.x((stageWidth - width) / 2);
    selectedImage.value.y((stageHeight - height) / 2);
    layer.value.draw();
  }
};

const handleScale = (direction) => {
  if (selectedImage.value) {
    const scale = direction > 0 ? 1.1 : 0.9;
    selectedImage.value.scaleX(selectedImage.value.scaleX() * scale);
    selectedImage.value.scaleY(selectedImage.value.scaleY() * scale);
    layer.value.draw();
  }
};

const handleScaleInput = () => {
  if (selectedImage.value) {
    const scale = prompt('Nhập tỷ lệ scale (0.1 - 10):', selectedImage.value.scaleX());
    if (scale !== null) {
      const scaleValue = parseFloat(scale);
      if (!isNaN(scaleValue) && scaleValue >= 0.1 && scaleValue <= 10) {
        selectedImage.value.scaleX(scaleValue);
        selectedImage.value.scaleY(scaleValue);
        layer.value.draw();
      }
    }
  }
};

const handleRotate = (angle) => {
  if (selectedImage.value) {
    selectedImage.value.rotation(selectedImage.value.rotation() + angle);
    layer.value.draw();
  }
};

const handleRotateInput = (event) => {
  if (selectedImage.value) {
    const angle = parseFloat(event.target.value);
    if (!isNaN(angle)) {
      selectedImage.value.rotation(angle);
      layer.value.draw();
    }
  }
};

const handleMove = (direction) => {
  if (selectedImage.value) {
    const step = 10;
    switch (direction) {
      case 'up':
        selectedImage.value.y(selectedImage.value.y() - step);
        break;
      case 'down':
        selectedImage.value.y(selectedImage.value.y() + step);
        break;
      case 'left':
        selectedImage.value.x(selectedImage.value.x() - step);
        break;
      case 'right':
        selectedImage.value.x(selectedImage.value.x() + step);
        break;
    }
    layer.value.draw();
  }
};

const handleFlip = (direction) => {
  if (selectedImage.value) {
    if (direction === 'horizontal') {
      selectedImage.value.scaleX(-selectedImage.value.scaleX());
    } else {
      selectedImage.value.scaleY(-selectedImage.value.scaleY());
    }
    layer.value.draw();
  }
};

const handleMouseEnter = () => {
  document.body.style.cursor = 'pointer';
};

const handleMouseLeave = () => {
  document.body.style.cursor = 'default';
};

// Thêm hàm chuyển đổi Konva sang texture
const convertKonvaToTexture = (konvaImage) => {
  // Tạo canvas tạm thời
  const tempCanvas = document.createElement('canvas');
  const tempContext = tempCanvas.getContext('2d');
  
  // Lấy kích thước của ảnh Konva
  tempCanvas.width = konvaImage.width() * konvaImage.scaleX();
  tempCanvas.height = konvaImage.height() * konvaImage.scaleY();
  
  // Vẽ ảnh Konva lên canvas tạm
  tempContext.drawImage(konvaImage.image(), 0, 0, tempCanvas.width, tempCanvas.height);
  
  // Tạo texture từ canvas
  const texture = new THREE.Texture(tempCanvas);
  texture.needsUpdate = true;
  
  return texture;
};

// Thêm hàm để xác định vị trí tương ứng giữa 2D và 3D
const getCorrespondingMesh = (konvaImage) => {
  if (!model || !konvaImage) return null;

  // Lấy vị trí và kích thước của ảnh trên canvas 2D
  const imageX = konvaImage.x() / stageConfig.value.width;
  const imageY = konvaImage.y() / stageConfig.value.height;
  const imageWidth = konvaImage.width() * konvaImage.scaleX() / stageConfig.value.width;
  const imageHeight = konvaImage.height() * konvaImage.scaleY() / stageConfig.value.height;

  // Tính toán vùng trung tâm của ảnh
  const imageCenterX = imageX + (imageWidth / 2);
  const imageCenterY = imageY + (imageHeight / 2);

  // Tìm mesh tương ứng dựa trên vị trí và kích thước
  let bestMatch = null;
  let minDistance = Infinity;

  model.traverse((child) => {
    if (child.isMesh) {
      // Lấy vị trí của mesh trong không gian 3D
      const meshPosition = child.position.clone();
      meshPosition.applyMatrix4(child.matrixWorld);

      // Chuyển đổi vị trí 3D sang tọa độ 2D
      const vector = meshPosition.clone();
      vector.project(camera);
      const meshX = (vector.x + 1) / 2;
      const meshY = (-vector.y + 1) / 2;

      // Tính toán kích thước của mesh trên màn hình
      const meshBox = new THREE.Box3().setFromObject(child);
      const meshSize = new THREE.Vector3();
      meshBox.getSize(meshSize);
      
      // Chuyển đổi kích thước 3D sang 2D
      const meshWidth = meshSize.x * camera.aspect;
      const meshHeight = meshSize.y;

      // Tính khoảng cách từ tâm mesh đến tâm ảnh
      const distance = Math.sqrt(
        Math.pow(meshX - imageCenterX, 2) + 
        Math.pow(meshY - imageCenterY, 2)
      );

      // Kiểm tra xem mesh có nằm trong vùng ảnh không
      const isInImageBounds = 
        meshX >= imageX && 
        meshX <= imageX + imageWidth &&
        meshY >= imageY && 
        meshY <= imageY + imageHeight;

      // Tính tỷ lệ kích thước giữa mesh và ảnh
      const sizeRatio = Math.min(
        meshWidth / (imageWidth * stageConfig.value.width),
        meshHeight / (imageHeight * stageConfig.value.height)
      );

      // Nếu mesh nằm trong vùng ảnh và có kích thước phù hợp
      if (isInImageBounds && sizeRatio > 0.3 && sizeRatio < 3) {
        if (distance < minDistance) {
          minDistance = distance;
          bestMatch = child;
        }
      }
    }
  });

  return bestMatch;
};

// Cập nhật hàm handleAddImageTo2D
const handleAddImageTo2D = (imageData) => {
  if (!stage.value || !layer.value) return;
  
  const image = new Image();
  image.src = imageData.url;
  
  image.onload = () => {
    // Tính toán kích thước phù hợp - giảm xuống 20% kích thước canvas
    const maxWidth = stageConfig.value.width * 0.2;
    const maxHeight = stageConfig.value.height * 0.2;
    
    let width = image.width;
    let height = image.height;
    
    // Tính tỷ lệ khung hình
    const aspectRatio = width / height;
    
    // Điều chỉnh kích thước nếu quá lớn
    if (width > maxWidth) {
      width = maxWidth;
      height = width / aspectRatio;
    }
    if (height > maxHeight) {
      height = maxHeight;
      width = height * aspectRatio;
    }

    // Tính toán vị trí ban đầu ở giữa canvas
    const x = (stageConfig.value.width - width) / 2;
    const y = (stageConfig.value.height - height) / 2;

    const konvaImage = new Konva.Image({
      x: x,
      y: y,
      image: image,
      width: width,
      height: height,
      draggable: true,
      id: `image-${imageData.timestamp}`,
      cornerRadius: 5,
      cornerStrokeWidth: 2,
      cornerStrokeColor: '#1a3c6c',
      cornerColor: '#ffffff',
      shadowColor: 'black',
      shadowBlur: 10,
      shadowOffset: { x: 5, y: 5 },
      shadowOpacity: 0.3,
      opacity: 0.7
    });

    // Thêm transformer
    const tr = new Konva.Transformer({
      nodes: [konvaImage],
      keepRatio: true,
      enabledAnchors: ['top-left', 'top-right', 'bottom-left', 'bottom-right']
    });

    layer.value.add(konvaImage);
    layer.value.add(tr);
    
    // Thêm sự kiện click và transform
    konvaImage.on('click', handleImageClick);
    
    // Tự động áp dụng texture vào mesh tương ứng khi ảnh được thêm vào
    const targetMesh = getCorrespondingMesh(konvaImage);
    if (targetMesh) {
      const texture = convertKonvaToTexture(konvaImage);
      
      // Lưu material gốc nếu chưa có
      if (!originalMaterials.has(targetMesh)) {
        originalMaterials.set(targetMesh, targetMesh.material.clone());
      }
      
      // Tạo material mới với texture
      const newMaterial = new THREE.MeshStandardMaterial({
        map: texture,
        metalness: 0,
        roughness: 0.5,
        transparent: true,
        opacity: 1,
        side: THREE.DoubleSide
      });
      
      // Áp dụng material mới
      targetMesh.material.dispose();
      targetMesh.material = newMaterial;
      targetMesh.material.needsUpdate = true;
    }
    
    // Thêm sự kiện transformend để cập nhật texture khi ảnh được điều chỉnh
    konvaImage.on('transformend', () => {
      const updatedTargetMesh = getCorrespondingMesh(konvaImage);
      if (updatedTargetMesh) {
        const updatedTexture = convertKonvaToTexture(konvaImage);
        updatedTargetMesh.material.map = updatedTexture;
        updatedTargetMesh.material.needsUpdate = true;
      }
    });
    
    layer.value.draw();
    
    // Chọn ảnh mới thêm vào
    selectedImage.value = konvaImage;
    currentTexture.value = imageData.url;
  };
};

// Thêm các hàm xử lý cho Konva
const handleTransformEnd = (e) => {
  const node = e.target;
  node.scaleX(Math.abs(node.scaleX()));
  node.scaleY(Math.abs(node.scaleY()));
  
  // Cập nhật vị trí và kích thước của ảnh
  const index = uploadedImages.value.findIndex(img => img.id === node.id());
  if (index !== -1) {
    uploadedImages.value[index] = {
      ...uploadedImages.value[index],
      x: node.x(),
      y: node.y(),
      width: node.width() * node.scaleX(),
      height: node.height() * node.scaleY(),
      rotation: node.rotation()
    };
  }
};

const handleImageClick = (e) => {
  const clickedOnTransformer = e.target.getParent().className === 'Transformer';
  if (clickedOnTransformer) {
    return;
  }

  const clickedOnImage = e.target.getType() === 'Image';
  if (clickedOnImage) {
    selectedImage.value = e.target;
    currentTexture.value = e.target.image().src;
    
    // Cập nhật transformer
    const tr = e.target.getParent().findOne('.Transformer');
    if (tr) {
      tr.nodes([e.target]);
      tr.getLayer().batchDraw();
    }
  } else {
    selectedImage.value = null;
    currentTexture.value = null;
  }
};

const handleImageSelected = (imageData) => {
  console.log('Image selected:', imageData); // Thêm log để debug
  currentTexture.value = imageData.url;
  selectedImage.value = imageData;
  handleAddImageTo2D(imageData);
};

// Thêm hàm xử lý text được thêm vào canvas
const handleTextAddedToCanvas = (textObj) => {
  if (!stage.value || !layer.value) return;
  
  // Tạo đối tượng Konva Text
  const konvaText = new Konva.Text({
    x: textObj.x,
    y: textObj.y,
    text: textObj.text,
    fontSize: textObj.fontSize,
    fontFamily: textObj.fontFamily,
    fill: textObj.color,
    fontWeight: textObj.fontWeight,
    fontStyle: textObj.fontStyle,
    textDecoration: textObj.textDecoration,
    align: textObj.align,
    width: textObj.width,
    height: textObj.height,
    draggable: true,
    rotation: textObj.rotation,
    id: textObj.id,
    cornerRadius: 5,
    cornerStrokeWidth: 2,
    cornerStrokeColor: '#1a3c6c',
    cornerColor: '#ffffff',
    shadowColor: 'black',
    shadowBlur: 10,
    shadowOffset: { x: 5, y: 5 },
    shadowOpacity: 0.3
  });
  
  // Thêm transformer
  const tr = new Konva.Transformer({
    nodes: [konvaText],
    keepRatio: false,
    enabledAnchors: ['top-left', 'top-right', 'bottom-left', 'bottom-right', 'middle-left', 'middle-right', 'top-center', 'bottom-center']
  });
  
  // Thêm vào layer
  layer.value.add(konvaText);
  layer.value.add(tr);
  
  // Thêm sự kiện click và transform
  konvaText.on('click', handleTextClick);
  konvaText.on('transformend', handleTextTransformEnd);
  
  // Lưu vào mảng textObjects
  textObjects.value.push({
    konvaObj: konvaText,
    transformer: tr,
    data: textObj
  });
  
  // Vẽ lại layer
  layer.value.draw();
  
  // Chọn text mới thêm vào
  selectedImage.value = konvaText;
  currentTexture.value = null;
};

// Xử lý khi click vào text
const handleTextClick = (e) => {
  const clickedOnTransformer = e.target.getParent().className === 'Transformer';
  if (clickedOnTransformer) {
    return;
  }
  
  const clickedOnText = e.target.getType() === 'Text';
  if (clickedOnText) {
    selectedImage.value = e.target;
    currentTexture.value = null;
    
    // Cập nhật transformer
    const tr = e.target.getParent().findOne('.Transformer');
    if (tr) {
      tr.nodes([e.target]);
      tr.getLayer().batchDraw();
    }
    
    // Cập nhật các giá trị cho công cụ chỉnh sửa text
    selectedTextColor.value = e.target.fill();
    selectedTextSize.value = e.target.fontSize();
    selectedTextFont.value = e.target.fontFamily();
    selectedTextBold.value = e.target.fontWeight() === 'bold';
    selectedTextItalic.value = e.target.fontStyle() === 'italic';
    selectedTextUnderline.value = e.target.textDecoration() === 'underline';
    selectedTextAlign.value = e.target.align();
    textRotation.value = e.target.rotation();
    textCurvature.value = 0; // Reset curvature when selecting new text
  } else {
    selectedImage.value = null;
    currentTexture.value = null;
  }
};

// Xử lý khi transform text kết thúc
const handleTextTransformEnd = (e) => {
  const node = e.target;
  
  // Cập nhật dữ liệu trong mảng textObjects
  const index = textObjects.value.findIndex(obj => obj.konvaObj === node);
  if (index !== -1) {
    textObjects.value[index].data.x = node.x();
    textObjects.value[index].data.y = node.y();
    textObjects.value[index].data.width = node.width() * node.scaleX();
    textObjects.value[index].data.height = node.height() * node.scaleY();
    textObjects.value[index].data.rotation = node.rotation();
  }
};

// Xử lý khi style của text được cập nhật
const handleTextStyleUpdated = (textObj) => {
  if (!layer.value) return;
  
  // Tìm đối tượng Konva Text tương ứng
  const index = textObjects.value.findIndex(obj => obj.data.id === textObj.id);
  if (index !== -1) {
    const konvaText = textObjects.value[index].konvaObj;
    
    // Cập nhật style
    konvaText.fontSize(textObj.fontSize);
    konvaText.fontFamily(textObj.fontFamily);
    konvaText.fill(textObj.color);
    konvaText.fontWeight(textObj.fontWeight);
    konvaText.fontStyle(textObj.fontStyle);
    konvaText.textDecoration(textObj.textDecoration);
    konvaText.align(textObj.align);
    
    // Vẽ lại layer
    layer.value.draw();
  }
};

// Thêm các hàm xử lý cho công cụ chỉnh sửa text
const updateTextStyle = () => {
  if (!selectedImage.value || selectedImage.value.getType() !== 'Text') return;
  
  // Cập nhật style cho text
  selectedImage.value.fill(selectedTextColor.value);
  selectedImage.value.fontSize(selectedTextSize.value);
  selectedImage.value.fontFamily(selectedTextFont.value);
  selectedImage.value.fontWeight(selectedTextBold.value ? 'bold' : 'normal');
  selectedImage.value.fontStyle(selectedTextItalic.value ? 'italic' : 'normal');
  selectedImage.value.textDecoration(selectedTextUnderline.value ? 'underline' : 'none');
  selectedImage.value.align(selectedTextAlign.value);
  
  // Vẽ lại layer
  layer.value.draw();
  
  // Cập nhật dữ liệu trong mảng textObjects
  const index = textObjects.value.findIndex(obj => obj.konvaObj === selectedImage.value);
  if (index !== -1) {
    textObjects.value[index].data.color = selectedTextColor.value;
    textObjects.value[index].data.fontSize = selectedTextSize.value;
    textObjects.value[index].data.fontFamily = selectedTextFont.value;
    textObjects.value[index].data.fontWeight = selectedTextBold.value ? 'bold' : 'normal';
    textObjects.value[index].data.fontStyle = selectedTextItalic.value ? 'italic' : 'normal';
    textObjects.value[index].data.textDecoration = selectedTextUnderline.value ? 'underline' : 'none';
    textObjects.value[index].data.align = selectedTextAlign.value;
  }
};

const toggleTextBold = () => {
  selectedTextBold.value = !selectedTextBold.value;
  updateTextStyle();
};

const toggleTextItalic = () => {
  selectedTextItalic.value = !selectedTextItalic.value;
  updateTextStyle();
};

const toggleTextUnderline = () => {
  selectedTextUnderline.value = !selectedTextUnderline.value;
  updateTextStyle();
};

const setTextAlign = (align) => {
  selectedTextAlign.value = align;
  updateTextStyle();
};

const handleRemoveText = () => {
  if (!selectedImage.value || selectedImage.value.getType() !== 'Text') return;
  
  // Xóa text khỏi layer
  selectedImage.value.destroy();
  
  // Xóa transformer
  const tr = layer.value.findOne('.Transformer');
  if (tr) {
    tr.destroy();
  }
  
  // Xóa khỏi mảng textObjects
  const index = textObjects.value.findIndex(obj => obj.konvaObj === selectedImage.value);
  if (index !== -1) {
    textObjects.value.splice(index, 1);
  }
  
  // Reset các giá trị
  selectedImage.value = null;
  currentTexture.value = null;
  
  // Vẽ lại layer
  layer.value.draw();
};

// Thêm các hàm xử lý mới cho text
const handleDuplicateText = () => {
  if (!selectedImage.value || selectedImage.value.getType() !== 'Text') return;
  
  const originalText = selectedImage.value;
  const clone = originalText.clone({
    x: originalText.x() + 20,
    y: originalText.y() + 20
  });
  
  layer.value.add(clone);
  
  // Thêm transformer cho text mới
  const tr = new Konva.Transformer({
    nodes: [clone],
    keepRatio: false,
    enabledAnchors: ['top-left', 'top-right', 'bottom-left', 'bottom-right', 'middle-left', 'middle-right', 'top-center', 'bottom-center']
  });
  layer.value.add(tr);
  
  // Thêm sự kiện cho text mới
  clone.on('click', handleTextClick);
  clone.on('transformend', handleTextTransformEnd);
  
  // Thêm vào mảng textObjects
  textObjects.value.push({
    konvaObj: clone,
    transformer: tr,
    data: { ...textObjects.value.find(obj => obj.konvaObj === originalText).data }
  });
  
  layer.value.draw();
  
  // Chọn text mới
  selectedImage.value = clone;
};

const handleEditText = () => {
  if (!selectedImage.value || selectedImage.value.getType() !== 'Text') return;
  
  const newText = prompt('Nhập văn bản mới:', selectedImage.value.text());
  if (newText !== null) {
    selectedImage.value.text(newText);
    layer.value.draw();
  }
};

const updateTextCurvature = () => {
  if (!selectedImage.value || selectedImage.value.getType() !== 'Text') return;
  
  // Implement text curvature logic here
  // This might require custom text path or other advanced Konva features
};

const handleTextFill = () => {
  if (!selectedImage.value || selectedImage.value.getType() !== 'Text') return;
  
  const stageWidth = stageConfig.value.width;
  const stageHeight = stageConfig.value.height;
  selectedImage.value.width(stageWidth);
  selectedImage.value.height(stageHeight);
  selectedImage.value.x(0);
  selectedImage.value.y(0);
  layer.value.draw();
};

const handleTextFit = () => {
  if (!selectedImage.value || selectedImage.value.getType() !== 'Text') return;
  
  const padding = 20;
  const stageWidth = stageConfig.value.width - (padding * 2);
  const stageHeight = stageConfig.value.height - (padding * 2);
  
  const textWidth = selectedImage.value.width() * selectedImage.value.scaleX();
  const textHeight = selectedImage.value.height() * selectedImage.value.scaleY();
  
  const scale = Math.min(
    stageWidth / textWidth,
    stageHeight / textHeight
  );
  
  selectedImage.value.scale({ x: scale, y: scale });
  selectedImage.value.position({
    x: (stageConfig.value.width - textWidth * scale) / 2,
    y: (stageConfig.value.height - textHeight * scale) / 2
  });
  
  layer.value.draw();
};

const handleTextScale = (direction) => {
  if (!selectedImage.value || selectedImage.value.getType() !== 'Text') return;
  
  const scale = direction > 0 ? 1.1 : 0.9;
  selectedImage.value.scaleX(selectedImage.value.scaleX() * scale);
  selectedImage.value.scaleY(selectedImage.value.scaleY() * scale);
  layer.value.draw();
};

const handleTextScaleInput = () => {
  if (!selectedImage.value || selectedImage.value.getType() !== 'Text') return;
  
  const scale = prompt('Nhập tỷ lệ scale (0.1 - 10):', selectedImage.value.scaleX());
  if (scale !== null) {
    const scaleValue = parseFloat(scale);
    if (!isNaN(scaleValue) && scaleValue >= 0.1 && scaleValue <= 10) {
      selectedImage.value.scaleX(scaleValue);
      selectedImage.value.scaleY(scaleValue);
      layer.value.draw();
    }
  }
};

const handleTextRotate = (angle) => {
  if (!selectedImage.value || selectedImage.value.getType() !== 'Text') return;
  
  const newRotation = selectedImage.value.rotation() + angle;
  selectedImage.value.rotation(newRotation);
  textRotation.value = newRotation;
  layer.value.draw();
};

const handleTextRotateInput = () => {
  if (!selectedImage.value || selectedImage.value.getType() !== 'Text') return;
  
  const angle = parseFloat(textRotation.value);
  if (!isNaN(angle)) {
    selectedImage.value.rotation(angle);
    layer.value.draw();
  }
};

const handleTextMove = (direction) => {
  if (!selectedImage.value || selectedImage.value.getType() !== 'Text') return;
  
  const step = 10;
  switch (direction) {
    case 'up':
      selectedImage.value.y(selectedImage.value.y() - step);
      break;
    case 'down':
      selectedImage.value.y(selectedImage.value.y() + step);
      break;
    case 'left':
      selectedImage.value.x(selectedImage.value.x() - step);
      break;
    case 'right':
      selectedImage.value.x(selectedImage.value.x() + step);
      break;
  }
  layer.value.draw();
};

const handleTextFlip = (direction) => {
  if (!selectedImage.value || selectedImage.value.getType() !== 'Text') return;
  
  if (direction === 'horizontal') {
    selectedImage.value.scaleX(-selectedImage.value.scaleX());
  } else {
    selectedImage.value.scaleY(-selectedImage.value.scaleY());
  }
  layer.value.draw();
};

// Thêm các hàm xử lý color picker
const openColorPicker = () => {
  if (!selectedImage.value || selectedImage.value.getType() !== 'Text') return;
  tempColor.value = selectedTextColor.value;
  showColorPicker.value = true;
};

const closeColorPicker = () => {
  if (!selectedImage.value || selectedImage.value.getType() !== 'Text') return;
  selectedTextColor.value = tempColor.value;
  showColorPicker.value = false;
};

const handleColorChange = (event) => {
  if (!selectedImage.value || selectedImage.value.getType() !== 'Text') return;
  selectedTextColor.value = event.target.value;
  // Cập nhật màu text ngay lập tức
  selectedImage.value.fill(event.target.value);
  layer.value.draw();
};

const applyColor = () => {
  if (!selectedImage.value || selectedImage.value.getType() !== 'Text') return;
  // Cập nhật màu text
  selectedImage.value.fill(selectedTextColor.value);
  layer.value.draw();
  showColorPicker.value = false;
  
  // Cập nhật dữ liệu trong mảng textObjects
  const index = textObjects.value.findIndex(obj => obj.konvaObj === selectedImage.value);
  if (index !== -1) {
    textObjects.value[index].data.color = selectedTextColor.value;
  }
};
</script>

<style scoped>
/* General Layout */
.page-wrapper {
  min-height: 100vh;
  background: linear-gradient(135deg, #f5f7fa 0%, #e0e8f5 100%);
  display: flex;
  flex-direction: column;
}

/* Main Content */
.main-content {
  flex: 1;
  padding: 2rem;
  max-width: 1400px;
  margin: 0 auto;
}

/* Customizer Grid */
.customizer-grid {
  display: grid;
  grid-template-columns: 300px 1fr 300px;
  gap: 2rem;
  align-items: flex-start;
  width: 100%;
  position: relative;
}

/* Product Area */
.product-area {
  grid-column: 2 / 3;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 2rem;
  margin-top: 1rem;
}

.product-preview {
  width: 100%;
  max-width: 600px;
  background: #ffffff;
  border-radius: 16px;
  box-shadow: 0 8px 30px rgba(0, 0, 0, 0.1);
  padding: 1rem;
}

.three-canvas {
  width: 100%;
  height: 400px;
  background-color: #ffffff;
  border-radius: 12px;
  overflow: hidden;
  position: relative;
  transition: all 0.3s ease;
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
  flex-direction: column;
  justify-content: center;
  align-items: center;
  border-radius: 12px;
}

.loading-spinner {
  width: 40px;
  height: 40px;
  border: 4px solid #f3f3f3;
  border-top: 4px solid #1a3c6c;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin-bottom: 1rem;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.loading-text {
  font-size: 1.2rem;
  color: #1a3c6c;
  font-weight: 600;
}

/* Drag Overlay */
.drag-overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(26, 60, 108, 0.9);
  display: flex;
  justify-content: center;
  align-items: center;
  border-radius: 12px;
}

.drop-zone {
  text-align: center;
  color: white;
  padding: 2rem;
  border: 2px dashed white;
  border-radius: 12px;
}

.upload-icon {
  font-size: 3rem;
  margin-bottom: 1rem;
  display: block;
}

/* Animation Toggle Button */
.animation-toggle-btn {
  position: absolute;
  bottom: 20px;
  left: 50%;
  transform: translateX(-50%);
  padding: 0.8rem 1.5rem;
  font-size: 0.9rem;
  font-weight: 600;
  color: #ffffff;
  background-color: #1a3c6c;
  border: none;
  border-radius: 25px;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  transition: all 0.3s ease;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
}

.animation-toggle-btn:hover {
  background-color: #2858a0;
  transform: translateX(-50%) scale(1.05);
}

.animation-icon {
  width: 12px;
  height: 12px;
  background: currentColor;
  position: relative;
}

.animation-icon.play::before {
  content: '';
  position: absolute;
  left: 4px;
  top: -4px;
  border-top: 10px solid transparent;
  border-bottom: 10px solid transparent;
  border-left: 15px solid currentColor;
}

.animation-icon.pause {
  display: flex;
  gap: 2px;
}

.animation-icon.pause::before,
.animation-icon.pause::after {
  content: '';
  width: 4px;
  height: 20px;
  background: currentColor;
}

/* Edit Tools Section */
.edit-tools-section {
  grid-column: 3 / 4;
  background: #ffffff;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  height: fit-content;
  max-width: 300px;
  margin: 0 auto;
}

.tool-group {
  border-bottom: 1px solid #eaeaea;
  padding: 8px;
}

.tool-group:last-child {
  border-bottom: none;
}

.tool-item {
  padding: 12px;
  border-bottom: 1px solid #eee;
}

.tool-item:last-child {
  border-bottom: none;
}

.tool-label {
  font-size: 14px;
  color: #333;
  margin-bottom: 8px;
}

.tool-value.good {
  color: #4CAF50;
  font-size: 14px;
}

.tool-actions {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.tool-btn {
  width: 100%;
  padding: 8px;
  background: #fff;
  border: 1px solid #ddd;
  border-radius: 4px;
  color: #333;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 8px;
}

.tool-btn:hover {
  background: #f5f5f5;
}

.remove-btn {
  background: #fff;
  color: #333;
}

.duplicate-btn {
  background: #fff;
  color: #333;
}

/* Controls */
.scale-controls,
.rotate-controls,
.move-controls,
.flip-controls {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 8px;
}

.move-controls {
  grid-template-columns: repeat(4, 1fr);
}

.rotate-input {
  width: 100%;
  padding: 8px;
  border: 1px solid #ddd;
  border-radius: 4px;
  text-align: center;
}

.tool-icon {
  font-size: 16px;
}

/* 2D Sneaker Section */
.sneaker-2d-section {
  width: 600px;
  height: 400px;
  background: #ffffff;
  padding: 1rem;
  border-radius: 16px;
  box-shadow: 0 8px 30px rgba(0, 0, 0, 0.1);
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto;
}

.canvas-container {
  width: 500px;
  height: 400px;
  border-radius: 8px;
  overflow: hidden;
  background: #ffffff;
  display: flex;
  align-items: center;
  justify-content: center;
}

/* Empty State */
.empty-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 2.5rem;
  text-align: center;
  height: 200px;
  background: #f8f9fa;
  border-radius: 12px;
  border: 2px dashed #dee2e6;
  transition: all 0.3s ease;
}

.empty-state:hover {
  border-color: #1976d2;
  background: #f1f8ff;
}

.empty-state-icon {
  font-size: 2.5rem;
  margin-bottom: 1rem;
  color: #6c757d;
  animation: bounce 2s infinite;
}

@keyframes bounce {
  0%, 100% { transform: translateY(0); }
  50% { transform: translateY(-10px); }
}

.empty-state-text {
  font-size: 1.1rem;
  color: #495057;
  font-weight: 500;
  line-height: 1.5;
}

/* Responsive Design */
@media (max-width: 1200px) {
  .customizer-grid {
    grid-template-columns: 250px 1fr 1fr 250px;
  }
}

@media (max-width: 992px) {
  .customizer-grid {
    grid-template-columns: 1fr;
    gap: 1rem;
  }

  .product-area,
  .edit-tools-section,
  .sneaker-2d-section {
    grid-column: 1 / -1;
  }

  .product-preview {
    max-width: 100%;
  }
}

@media (max-width: 768px) {
  .main-content {
    padding: 1rem;
  }

  .tool-actions {
    flex-direction: column;
  }

  .tool-btn {
    width: 100%;
  }
}

/* Thêm style cho công cụ chỉnh sửa text */
.text-edit-tools {
  padding: 1rem;
  background: #f8f9fa;
  border-radius: 8px;
  margin-bottom: 1rem;
}

.tools-title {
  font-size: 1rem;
  margin: 0 0 1rem 0;
  color: #333;
  text-align: center;
}

.range-control {
  display: flex;
  align-items: center;
  gap: 10px;
}

.range-control input[type="range"] {
  flex: 1;
  height: 4px;
}

.range-value {
  min-width: 30px;
  text-align: right;
  color: #666;
}

.font-style-buttons, .align-buttons {
  display: flex;
  gap: 0.5rem;
}

.font-style-buttons button, .align-buttons button {
  flex: 1;
  padding: 0.5rem;
  background: #fff;
  border: 1px solid #e0e0e0;
  border-radius: 4px;
  color: #333;
  cursor: pointer;
  transition: all 0.2s ease;
}

.font-style-buttons button:hover, .align-buttons button:hover {
  background: #f5f5f5;
}

.font-style-buttons button.active, .align-buttons button.active {
  background: #e3f2fd;
  border: 1px solid #2196f3;
}

.font-style-buttons button.active strong,
.font-style-buttons button.active em,
.font-style-buttons button.active u {
  color: #2196f3;
}

/* Thêm style mới */
.font-select {
  width: 100%;
  padding: 8px;
  border: 1px solid #ddd;
  border-radius: 4px;
  background: #fff;
}

.color-picker {
  width: 100%;
  height: 32px;
  padding: 2px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.size-input {
  width: 100%;
  padding: 8px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

/* Cập nhật style cho công cụ chỉnh sửa text */
.tool-item {
  padding: 12px;
  border-bottom: 1px solid #eee;
}

.tool-item:last-child {
  border-bottom: none;
}

.tool-label {
  font-size: 14px;
  color: #333;
  margin-bottom: 8px;
}

.font-display {
  padding: 8px;
  background: #f5f5f5;
  border: 1px solid #ddd;
  border-radius: 4px;
  color: #333;
}

.color-display {
  width: 100%;
  height: 32px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.size-display {
  padding: 8px;
  background: #f5f5f5;
  border: 1px solid #ddd;
  border-radius: 4px;
  color: #333;
}

.range-control {
  display: flex;
  align-items: center;
  gap: 10px;
}

.range-slider {
  flex: 1;
  height: 4px;
  -webkit-appearance: none;
  background: #ddd;
  border-radius: 2px;
}

.range-slider::-webkit-slider-thumb {
  -webkit-appearance: none;
  width: 16px;
  height: 16px;
  background: #fff;
  border: 1px solid #ddd;
  border-radius: 50%;
  cursor: pointer;
}

.range-value {
  min-width: 30px;
  text-align: right;
  color: #666;
}

.tool-btn {
  width: 100%;
  padding: 8px;
  background: #fff;
  border: 1px solid #ddd;
  border-radius: 4px;
  color: #333;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  margin-bottom: 8px;
  font-size: 14px;
}

.tool-btn:last-child {
  margin-bottom: 0;
}

.tool-btn:hover {
  background: #f5f5f5;
}

.scale-controls,
.rotate-controls,
.move-controls,
.flip-controls {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 8px;
}

.move-controls {
  grid-template-columns: repeat(4, 1fr);
}

.rotate-input {
  width: 100%;
  padding: 8px;
  border: 1px solid #ddd;
  border-radius: 4px;
  text-align: center;
}

.tool-actions {
  display: flex;
  gap: 8px;
}

.tool-actions .tool-btn {
  flex: 1;
  margin: 0;
}

/* Style cho color picker modal */
.color-picker-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

.color-picker-content {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  width: 300px;
  overflow: hidden;
}

.color-picker-header {
  padding: 16px;
  background: #f5f5f5;
  border-bottom: 1px solid #ddd;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.close-btn {
  background: none;
  border: none;
  font-size: 24px;
  cursor: pointer;
  color: #666;
}

.color-picker-body {
  padding: 20px;
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.color-preview {
  width: 100%;
  height: 60px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.color-picker-input {
  width: 100%;
  height: 40px;
  padding: 0;
  border: 1px solid #ddd;
  border-radius: 4px;
  cursor: pointer;
}

.color-picker-footer {
  padding: 16px;
  background: #f5f5f5;
  border-top: 1px solid #ddd;
  display: flex;
  justify-content: flex-end;
  gap: 8px;
}

.color-picker-btn {
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 500;
}

.color-picker-btn.cancel {
  background: #fff;
  border: 1px solid #ddd;
  color: #666;
}

.color-picker-btn.apply {
  background: #1a73e8;
  border: none;
  color: white;
}

.color-picker-btn:hover {
  opacity: 0.9;
}
</style>
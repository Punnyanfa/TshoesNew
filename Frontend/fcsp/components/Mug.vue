<template>
  <div v-if="error" class="error-message">
    {{ error }}
  </div>
</template>

<script setup>
import { ref, watch, onMounted, onUnmounted, defineExpose } from 'vue'
import * as THREE from 'three'
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader'

const props = defineProps({
  position: {
    type: Array,
    default: () => [0, 0, 0]
  },
  selectedPart: {
    type: String,
    default: ''
  },
  scene: {
    type: Object,
    required: true
  }
})

const emit = defineEmits(['colorChange', 'modelLoaded', 'loadingProgress'])

const error = ref(null)
const loadingProgress = ref(0)
let model = null
const materials = {}

// Chỉ sử dụng GLTFLoader, không có DRACO
const loader = new GLTFLoader()

onMounted(() => {
  console.log('Bắt đầu tải mô hình...')
  loader.load(
    '/Adidasrunningshoes.glb',
    (gltf) => {
      console.log('Mô hình đã tải xong, đang xử lý...')
      model = gltf.scene
      model.position.set(...props.position)
      
      // Thay đổi kích thước nếu cần
      model.scale.set(1.5, 1.5, 1.5) // Phóng to mô hình
      
      // Xoay mô hình để nhìn rõ hơn
      model.rotation.y = Math.PI / 4 // Xoay 45 độ
      
      // In ra thông tin về các phần của mô hình để debug
      console.log('Danh sách các phần (mesh) trong mô hình:')
      model.traverse((node) => {
        if (node.isMesh) {
          console.log(`- Tên mesh: ${node.name}`)
          materials[node.name] = node.material
          
          // Đảm bảo material được thiết lập đúng
          if (!node.material) {
            node.material = new THREE.MeshStandardMaterial({ color: 0x808080 })
          }
        }
      })
      
      // Thêm model vào scene
      props.scene.add(model)
      emit('modelLoaded')
      console.log('Model loaded successfully:', model)
    },
    (xhr) => {
      loadingProgress.value = (xhr.loaded / xhr.total) * 100
      emit('loadingProgress', loadingProgress.value)
      console.log('Loading progress:', loadingProgress.value + '%')
    },
    (err) => {
      error.value = 'Không thể tải mô hình 3D. Vui lòng thử lại sau.'
      console.error('Error loading model:', err)
      console.error('Error details:', {
        message: err.message,
        stack: err.stack
      })
    }
  )
})

// Cleanup khi component bị hủy
onUnmounted(() => {
  if (model) {
    // Xóa model khỏi scene
    props.scene.remove(model)
    
    // Dọn dẹp tài nguyên
    model.traverse((node) => {
      if (node.isMesh) {
        if (node.geometry) node.geometry.dispose()
        if (node.material) {
          if (Array.isArray(node.material)) {
            node.material.forEach(material => material.dispose())
          } else {
            node.material.dispose()
          }
        }
      }
    })
  }
})

// Hàm cập nhật màu sắc
const updateSelectedPartColor = (color) => {
  if (props.selectedPart && materials[props.selectedPart]) {
    materials[props.selectedPart].color.set(color)
  }
}

// Theo dõi thay đổi của selectedPart
watch(() => props.selectedPart, (newPart) => {
  if (newPart && materials[newPart]) {
    emit('colorChange', (color) => {
      updateSelectedPartColor(color)
    })
  }
})

defineExpose({
  updateSelectedPartColor
})
</script>

<style scoped>
.error-message {
  color: red;
  text-align: center;
  padding: 20px;
  background: rgba(255, 0, 0, 0.1);
  border-radius: 4px;
  margin: 10px;
}
</style> 
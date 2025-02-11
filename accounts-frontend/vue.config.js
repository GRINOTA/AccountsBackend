const { defineConfig } = require('@vue/cli-service')

module.exports = defineConfig({
  devServer: {
    proxy: {
      '/api': {
          target: 'http://localhost:5244',
          ws: true,
          changeOrigin: true
      }
    }
  } 
})


// module.exports = defineConfig({
//   transpileDependencies: true
// })

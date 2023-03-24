import {defineConfig, loadEnv} from 'vite'
import vue from '@vitejs/plugin-vue'
import {resolve} from 'path'
import {readFileSync} from 'fs'

export default ({mode}) => {
    process.env = {...process.env, ...loadEnv(mode, process.cwd())};
    
    return defineConfig({
        resolve: {
            alias: {
                '@': resolve(__dirname, 'src')
            }
        },
        server: {
            https: {
                cert: readFileSync(process.env.VITE_SSL_CRT_FILE),
                key: readFileSync(process.env.VITE_SSL_KEY_FILE)
            }
        },
        plugins: [vue()]
    });
}
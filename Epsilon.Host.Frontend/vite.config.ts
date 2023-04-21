import { defineConfig, loadEnv, UserConfigExport } from "vite"
import vue from "@vitejs/plugin-vue"
import { resolve } from "path"
import { readFileSync } from "fs"

export default ({ mode }: { mode: string }): UserConfigExport => {
    process.env = { ...process.env, ...loadEnv(mode, process.cwd()) }
    const certificate = process.env.VITE_SSL_CRT_FILE
    const key = process.env.VITE_SSL_KEY_FILE

    return defineConfig({
        resolve: {
            alias: {
                "@": resolve(__dirname, "src"),
            },
        },
        server: {
            https: {
                cert: certificate ? readFileSync(certificate) : undefined,
                key: key ? readFileSync(key) : undefined,
            },
        },
        plugins: [vue()],
    })
}

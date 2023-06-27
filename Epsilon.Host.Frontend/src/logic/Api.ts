/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export interface Activity {
    /** @format int32 */
    id?: number
    name?: string | null
    shortName?: string | null
    color?: string | null
}

export interface ArchitectureLayer {
    /** @format int32 */
    id?: number
    name?: string | null
    shortName?: string | null
    color?: string | null
}

export interface CompetenceProfile {
    hboIDomain?: IHboIDomain
    professionalTaskOutcomes?: ProfessionalTaskResult[] | null
    professionalSkillOutcomes?: ProfessionalSkillResult[] | null
}

export interface EnrollmentTerm {
    name?: string | null
    /** @format date-time */
    start_at?: string | null
    /** @format date-time */
    end_at?: string | null
}

export interface IHboIDomain {
    architectureLayers?: ArchitectureLayer[] | null
    activities?: Activity[] | null
    professionalSkills?: ProfessionalSkill[] | null
    masteryLevels?: MasteryLevel[] | null
}

export interface MasteryLevel {
    /** @format int32 */
    id?: number
    /** @format int32 */
    level?: number
    color?: string | null
}

export interface ProfessionalSkill {
    /** @format int32 */
    id?: number
    name?: string | null
    shortName?: string | null
    color?: string | null
}

export interface ProfessionalSkillResult {
    /** @format int32 */
    outcomeId?: number
    /** @format double */
    grade?: number
    /** @format date-time */
    submittedAt?: string
    /** @format int32 */
    skill?: number
    /** @format int32 */
    masteryLevel?: number
    /** @format date-time */
    assessedAt?: string
}

export interface ProfessionalTaskResult {
    /** @format int32 */
    outcomeId?: number
    /** @format double */
    grade?: number
    /** @format date-time */
    submittedAt?: string
    /** @format int32 */
    architectureLayer?: number
    /** @format int32 */
    activity?: number
    /** @format int32 */
    masteryLevel?: number
    /** @format date-time */
    assessedAt?: string
}

export interface User {
    _id?: string | null
    name?: string | null
    /** @format uri */
    avatarUrl?: string | null
}

export type QueryParamsType = Record<string | number, any>
export type ResponseFormat = keyof Omit<Body, "body" | "bodyUsed">

export interface FullRequestParams extends Omit<RequestInit, "body"> {
    /** set parameter to `true` for call `securityWorker` for this request */
    secure?: boolean
    /** request path */
    path: string
    /** content type of request body */
    type?: ContentType
    /** query params */
    query?: QueryParamsType
    /** format of response (i.e. response.json() -> format: "json") */
    format?: ResponseFormat
    /** request body */
    body?: unknown
    /** base url */
    baseUrl?: string
    /** request cancellation token */
    cancelToken?: CancelToken
}

export type RequestParams = Omit<FullRequestParams, "body" | "method" | "query" | "path">

export interface ApiConfig<SecurityDataType = unknown> {
    baseUrl?: string
    baseApiParams?: Omit<RequestParams, "baseUrl" | "cancelToken" | "signal">
    securityWorker?: (securityData: SecurityDataType | null) => Promise<RequestParams | void> | RequestParams | void
    customFetch?: typeof fetch
}

export interface HttpResponse<D extends unknown, E extends unknown = unknown> extends Response {
    data: D
    error: E
}

type CancelToken = Symbol | string | number

export enum ContentType {
    Json = "application/json",
    FormData = "multipart/form-data",
    UrlEncoded = "application/x-www-form-urlencoded",
    Text = "text/plain",
}

export class HttpClient<SecurityDataType = unknown> {
    public baseUrl: string = import.meta.env.VITE_EPSILON_API_ENDPOINT ?? "api"
    private securityData: SecurityDataType | null = null
    private securityWorker?: ApiConfig<SecurityDataType>["securityWorker"]
    private abortControllers = new Map<CancelToken, AbortController>()
    private customFetch = (...fetchParams: Parameters<typeof fetch>) => fetch(...fetchParams)

    private baseApiParams: RequestParams = {
        credentials: "same-origin",
        headers: {},
        redirect: "follow",
        referrerPolicy: "no-referrer",
    }

    constructor(apiConfig: ApiConfig<SecurityDataType> = {}) {
        Object.assign(this, apiConfig)
    }

    public setSecurityData = (data: SecurityDataType | null) => {
        this.securityData = data
    }

    protected encodeQueryParam(key: string, value: any) {
        const encodedKey = encodeURIComponent(key)
        return `${encodedKey}=${encodeURIComponent(typeof value === "number" ? value : `${value}`)}`
    }

    protected addQueryParam(query: QueryParamsType, key: string) {
        return this.encodeQueryParam(key, query[key])
    }

    protected addArrayQueryParam(query: QueryParamsType, key: string) {
        const value = query[key]
        return value.map((v: any) => this.encodeQueryParam(key, v)).join("&")
    }

    protected toQueryString(rawQuery?: QueryParamsType): string {
        const query = rawQuery || {}
        const keys = Object.keys(query).filter((key) => "undefined" !== typeof query[key])
        return keys
            .map((key) =>
                Array.isArray(query[key]) ? this.addArrayQueryParam(query, key) : this.addQueryParam(query, key)
            )
            .join("&")
    }

    protected addQueryParams(rawQuery?: QueryParamsType): string {
        const queryString = this.toQueryString(rawQuery)
        return queryString ? `?${queryString}` : ""
    }

    private contentFormatters: Record<ContentType, (input: any) => any> = {
        [ContentType.Json]: (input: any) =>
            input !== null && (typeof input === "object" || typeof input === "string") ? JSON.stringify(input) : input,
        [ContentType.Text]: (input: any) =>
            input !== null && typeof input !== "string" ? JSON.stringify(input) : input,
        [ContentType.FormData]: (input: any) =>
            Object.keys(input || {}).reduce((formData, key) => {
                const property = input[key]
                formData.append(
                    key,
                    property instanceof Blob
                        ? property
                        : typeof property === "object" && property !== null
                        ? JSON.stringify(property)
                        : `${property}`
                )
                return formData
            }, new FormData()),
        [ContentType.UrlEncoded]: (input: any) => this.toQueryString(input),
    }

    protected mergeRequestParams(params1: RequestParams, params2?: RequestParams): RequestParams {
        return {
            ...this.baseApiParams,
            ...params1,
            ...(params2 || {}),
            headers: {
                ...(this.baseApiParams.headers || {}),
                ...(params1.headers || {}),
                ...((params2 && params2.headers) || {}),
            },
        }
    }

    protected createAbortSignal = (cancelToken: CancelToken): AbortSignal | undefined => {
        if (this.abortControllers.has(cancelToken)) {
            const abortController = this.abortControllers.get(cancelToken)
            if (abortController) {
                return abortController.signal
            }
            return void 0
        }

        const abortController = new AbortController()
        this.abortControllers.set(cancelToken, abortController)
        return abortController.signal
    }

    public abortRequest = (cancelToken: CancelToken) => {
        const abortController = this.abortControllers.get(cancelToken)

        if (abortController) {
            abortController.abort()
            this.abortControllers.delete(cancelToken)
        }
    }

    public request = async <T = any, E = any>({
        body,
        secure,
        path,
        type,
        query,
        format,
        baseUrl,
        cancelToken,
        ...params
    }: FullRequestParams): Promise<HttpResponse<T, E>> => {
        const secureParams =
            ((typeof secure === "boolean" ? secure : this.baseApiParams.secure) &&
                this.securityWorker &&
                (await this.securityWorker(this.securityData))) ||
            {}
        const requestParams = this.mergeRequestParams(params, secureParams)
        const queryString = query && this.toQueryString(query)
        const payloadFormatter = this.contentFormatters[type || ContentType.Json]
        const responseFormat = format || requestParams.format

        return this.customFetch(`${baseUrl || this.baseUrl || ""}${path}${queryString ? `?${queryString}` : ""}`, {
            ...requestParams,
            headers: {
                ...(requestParams.headers || {}),
                ...(type && type !== ContentType.FormData ? { "Content-Type": type } : {}),
            },
            signal: cancelToken ? this.createAbortSignal(cancelToken) : requestParams.signal,
            body: typeof body === "undefined" || body === null ? null : payloadFormatter(body),
        }).then(async (response) => {
            const r = response as HttpResponse<T, E>
            r.data = null as unknown as T
            r.error = null as unknown as E

            const data = !responseFormat
                ? r
                : await response[responseFormat]()
                      .then((data) => {
                          if (r.ok) {
                              r.data = data
                          } else {
                              r.error = data
                          }
                          return r
                      })
                      .catch((e) => {
                          r.error = e
                          return r
                      })

            if (cancelToken) {
                this.abortControllers.delete(cancelToken)
            }

            if (!response.ok) throw data
            return data
        })
    }
}

/**
 * @title Epsilon.Host.WebApi
 * @version 1.0
 */
export class Api<SecurityDataType extends unknown> extends HttpClient<SecurityDataType> {
    auth = {
        /**
         * No description
         *
         * @tags Auth
         * @name ChallengeList
         * @request GET:/auth/challenge
         */
        challengeList: (params: RequestParams = {}) =>
            this.request<void, any>({
                path: `/auth/challenge`,
                method: "GET",
                ...params,
            }),

        /**
         * No description
         *
         * @tags Auth
         * @name CallbackList
         * @request GET:/auth/callback
         */
        callbackList: (params: RequestParams = {}) =>
            this.request<void, any>({
                path: `/auth/callback`,
                method: "GET",
                ...params,
            }),
    }
    component = {
        /**
         * No description
         *
         * @tags Component
         * @name ComponentDetail
         * @request GET:/component/{componentName}
         */
        componentDetail: (
            componentName: string,
            query?: {
                /** @format date-time */
                startDate?: string
                /** @format date-time */
                endDate?: string
            },
            params: RequestParams = {}
        ) =>
            this.request<CompetenceProfile, any>({
                path: `/component/${componentName}`,
                method: "GET",
                query: query,
                format: "json",
                ...params,
            }),
    }
    document = {
        /**
         * No description
         *
         * @tags Document
         * @name WordList
         * @request GET:/document/word
         */
        wordList: (
            query?: {
                /** @format date-time */
                startDate?: string
                /** @format date-time */
                endDate?: string
            },
            params: RequestParams = {}
        ) =>
            this.request<void, any>({
                path: `/document/word`,
                method: "GET",
                query: query,
                ...params,
            }),
    }
    filter = {
        /**
         * No description
         *
         * @tags Filter
         * @name ParticipatedTermsList
         * @request GET:/filter/participated-terms
         */
        participatedTermsList: (params: RequestParams = {}) =>
            this.request<EnrollmentTerm[], any>({
                path: `/filter/participated-terms`,
                method: "GET",
                format: "json",
                ...params,
            }),

        /**
         * No description
         *
         * @tags Filter
         * @name AccessibleStudentsList
         * @request GET:/filter/accessible-students
         */
        accessibleStudentsList: (params: RequestParams = {}) =>
            this.request<User[], any>({
                path: `/filter/accessible-students`,
                method: "GET",
                format: "json",
                ...params,
            }),
    }
}

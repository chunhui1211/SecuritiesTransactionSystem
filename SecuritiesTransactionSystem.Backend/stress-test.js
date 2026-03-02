import http from 'k6/http';
import { check, sleep } from 'k6';

// 設定壓力測試參數
export const options = {
    stages: [
        { duration: '10s', target: 10 },
        { duration: '20s', target: 50 }
    ],
    thresholds: {
        http_req_failed: ['rate<0.01'],
    },
};

const BASE_URL = 'https://localhost:7125/api/v1';

// 登入
export function setup() {
    const loginurl = `${BASE_URL}/auth/login`;
    const loginpayload = JSON.stringify({
        username: 'admin',
        password: '123456',
    });

    const loginparams = {
        headers: { 'content-type': 'application/json' },
    };

    const loginres = http.post(loginurl, loginpayload, loginparams);

    const logincheck = check(loginres, {
        'login success': (r) => r.status === 200,
        'has token': (r) => {
            const body = r.json();
            return body.payload && body.payload.token !== undefined && body.payload.token.length > 0;
        }
    });

    if (!logincheck) {
        throw new Error(`login failed: ${loginres.status} ${loginres.body}`);
    }

    const token = loginres.json('payload.token');

    return { token: token };
}

// 壓力測試
export default function (data) {
    const authParams = {
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${data.token}`,
        },
    };

    const url = `${BASE_URL}/orders`;
    const payload = JSON.stringify({ symbol: '2330', price: 2000, quantity: 1, side: 0 });
    const res = http.post(url, payload, authParams);

    check(res, { 'is status 200': (r) => r.status === 200 });

    sleep(0.5);
}
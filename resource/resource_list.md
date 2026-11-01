# 패링 액션 리소스 목록

경로는 `resource` 폴더 기준이며, 크기의 단위는 픽셀이다.

## 배경

| 리소스 | 경로 | 이미지 크기 | 설명 |
|---|---|---|---|
| 메인 메뉴 배경 | `background/title_background.png` | 960×540 | 주황색 조명이 있는 어두운 창고 내부 배경 |
| 플레이 배경 | `background/stage_background.png` | 1920×540 | 왼쪽 하역 구역과 오른쪽 설비 구역, 양 끝 통로와 평평한 바닥으로 구성한 배경 |

## 메인 메뉴

| 리소스 | 경로 | 이미지 크기 | 설명 |
|---|---|---|---|
| 타이틀 로고 | `menu/title_logo.png` | 384×128 | PARRY ACTION 영문 로고 |
| 버튼 기본 | `menu/menu_button_normal.png` | 256×128 | 회색 테두리의 기본 버튼 |
| 버튼 선택 | `menu/menu_button_selected.png` | 256×128 | 주황색 테두리로 강조한 선택 상태 |
| 버튼 눌림 | `menu/menu_button_pressed.png` | 256×128 | 내부를 어둡게 표현한 눌림 상태 |

## 플레이어

| 리소스 | 경로 | 시트 크기 | 프레임 크기 | 프레임 수 | 설명 |
|---|---|---|---|---:|---|
| 기준 자세 | `player/player_base.png` | 192×192 | 192×192 | 1 | 검을 들고 서 있는 기본 자세 |
| 대기 | `player/player_idle.png` | 768×192 | 192×192 | 4 | 제자리에서 대기하는 동작 |
| 이동 | `player/player_move.png` | 1152×192 | 192×192 | 6 | 검을 들고 달리는 동작 |
| 공격 | `player/player_attack.png` | 1152×192 | 192×192 | 6 | 검을 휘두르는 공격 동작 |
| 패링 | `player/player_parry.png` | 384×192 | 192×192 | 2 | 검으로 공격을 받아내는 동작 |
| 사망 | `player/player_dead.png` | 1536×192 | 192×192 | 8 | 뒤로 쓰러져 바닥에 눕는 동작 |

플레이어 시트는 투명 배경의 가로 한 줄이며, 총 27프레임이다.

## 사운드

| 리소스 | 경로 | 길이 | 설명 |
|---|---|---|---|
| 배경음악 | `audio/bgm/background.wav` | 38.400초 | Assembly Not Required — 어두운 창고 액션용 신스웨이브 배경음악 |
| 버튼 클릭 | `audio/sfx/button_click.wav` | 0.100초 | 메뉴 버튼을 누를 때 나는 짧은 클릭음 |
| 검 휘두름 | `audio/sfx/sword_swing.wav` | 0.581초 | 공격의 유효 구간이 시작될 때 나는 검 휘두름 |
| 패링 동작 | `audio/sfx/parry_start.wav` | 0.544초 | 패링 자세를 잡는 짧은 검 동작음 |
| 패링 성공 | `audio/sfx/parry_success.wav` | 1.324초 | 공격을 받아냈을 때 나는 금속 충돌음 |

### 사운드 출처 및 라이선스

| 게임 파일 | 원본 파일 | 제작자 | 출처 | 라이선스 |
|---|---|---|---|---|
| `audio/bgm/background.wav` | `assembly_not_required.ogg` | AndyGun11 | [Assembly Not Required](https://opengameart.org/content/assembly-not-required-dark-synthwave-electronic-body-music) | [CC BY 4.0](https://creativecommons.org/licenses/by/4.0/) |
| `audio/sfx/button_click.wav` | `Audio/click_001.ogg` | Kenney | [Interface Sounds](https://kenney.nl/assets/interface-sounds) | [CC0 1.0](https://creativecommons.org/publicdomain/zero/1.0/) |
| `audio/sfx/sword_swing.wav` | `sword.4.ogg` | StarNinjas | [20 Sword Sound Effects](https://opengameart.org/content/20-sword-sound-effects-attacks-and-clashes) | [CC0 1.0](https://creativecommons.org/publicdomain/zero/1.0/) |
| `audio/sfx/parry_start.wav` | `sword.6.ogg` | StarNinjas | [20 Sword Sound Effects](https://opengameart.org/content/20-sword-sound-effects-attacks-and-clashes) | [CC0 1.0](https://creativecommons.org/publicdomain/zero/1.0/) |
| `audio/sfx/parry_success.wav` | `sword_clash.6.ogg` | StarNinjas | [20 Sword Sound Effects](https://opengameart.org/content/20-sword-sound-effects-attacks-and-clashes) | [CC0 1.0](https://creativecommons.org/publicdomain/zero/1.0/) |

- BGM 저작자 표시: **Assembly Not Required by AndyGun11**.
- 변경 사항: 원본 OGG를 PCM 16비트 WAV로 변환하고 음량을 조정했다. 패링 성공음은 다른 동작보다 선명하게 설정했다.
- 원저작자가 이 게임을 보증하거나 후원한다는 의미는 아니다.
- BGM은 CC BY 4.0에 따라 저작자·출처·라이선스 링크·변경 사항을 표시한다. 효과음은 CC0 1.0이며, 출처도 함께 기록한다.
- 배경음은 메뉴와 플레이에서 같은 파일을 반복 재생한다. `parry_start`는 자세를 잡는 동작음이며, 금속 충돌음인 `parry_success`는 패링 성공 시에만 재생한다.
- 음원 교체 시 확장자만 바꾸지 말고 실제 PCM WAV 파일을 사용하며, 이 목록의 출처·라이선스·변경 사항도 갱신한다.
- 이 문서는 빌드·배포 시 `resource/resource_list.md`로 함께 복사한다.

# HalfGame Code
3학년 1학기에 진행하였던 '하프 게임' 중 제가 맡았던 부분의 코드와 UI를 정리한 레포지토리입니다.

------

## Auth
facebook 로그인 기능, google 로그인 기능을 구현하였습니다.

## Data
scriptable object를 이용하여 각종 정보들을 저장하였습니다.

## Event
게임 속 event들을 관리하고 호출할 수 있게 만드는 event manager입니다.

## Chat
- 게임 내 채팅 시스템을 구현하는데 이용된 스크립트들 입니다.
### ChatManager
부모 클래스인 ChatManager과 그것을 상속받는 WaitingRoomChatManager, GameChatManager로 나뉘어 있습니다.
게임 진행 이후에도 여전히 해당 대기방에 머무르는 기능을 수행하기 위하여, 한 Scene에서 대기방과 인게임 채팅 시스템이 동시에 켜져있는 경우를 고려해야합니다.
그렇기 때문에 스크립트를 분리하여 각자 가지고 있는 text field등의 변수를 구분하였습니다.

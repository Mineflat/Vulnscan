from scapy.layers.inet import ICMP, IP, TCP, sr1, conf
import sys
import socket


def icmp_probe(ip):
    icmp_packet = IP(dst=ip) / ICMP()
    # Отправка и прием одного пакета
    resp_packet = sr1(icmp_packet, timeout=10)
    return resp_packet is not None


def syn_scan(ip, ports):
    # Проходимся по каждому порту
    for port in ports:
        # Флаг S означает SYN-пакет
        syn_packet = IP(dst=ip) / TCP(dport=port, flags="S")
        # Время ожидания пакета можно ставить свое
        resp_packet = sr1(syn_packet, timeout=10)
        if resp_packet is not None:
            if resp_packet.getlayer('TCP').flags & 0x12 != 0:
                result = f"{ip}:{port} is open/{resp_packet.sprintf('%TCP.sport%')}"


if __name__ == "__main__":
    conf.verb = 0
    name = sys.argv[1]
    # Узнаем IP цели
    ip = socket.gethostbyname(name)
    # Обозначаем порты для сканирования
    ports = sys.argv[2]
    start, end = map(int, ports.split('-'))
    porter = range(start, end + 1)
    # Перехватываем исключения в момент, когда заканчивается кортеж
    try:
        # Если не удалось подключиться к серверу, выводим ошибку
        if icmp_probe(ip):
            syn_ack_packet = syn_scan(ip, porter)
            syn_ack_packet.show()
        else:
            error = "Failed to send ICMP packet"
    except AttributeError:
        comp = "Scan completed!"